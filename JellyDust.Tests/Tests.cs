using System;
using System.Data;
using System.Linq;
using FakeItEasy;
using JellyDust;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private IDbConnectionFactory _connectionFactory;
        private IDbConnection _connection;
        private IDbTransactionFactory _transactionFactory;
        private IDbTransaction _transaction;
        

        [SetUp]
        public void Setup()
        {
            int currentConnection = 0;
            int currentTransaction = 0;
            _connectionFactory = A.Fake<IDbConnectionFactory>();
            _connection = A.Fake<IDbConnection>();
            A.CallTo(() => _connectionFactory.CreateOpenConnection()).Returns(_connection);

            _transaction = A.Fake<IDbTransaction>();
            _transactionFactory = A.Fake<IDbTransactionFactory>();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).Returns(_transaction);


        }

        [Test]
        public void JustCommitDoesntCreateAnyConnectionNorTransaction()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                sut.Commit();
                Assert.IsFalse(sut.HasTransaction());
                Assert.IsFalse(sut.HasConnection());
                A.CallTo(() => _transaction.Commit()).MustNotHaveHappened();
            }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustNotHaveHappened();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).MustNotHaveHappened();

        }

        [Test]
        public void JustDisposingDoesntCreateAnyConnectionNotTransaction()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory)) { }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustNotHaveHappened();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void AskingForConnectionDoesNotCreateAnActualConnectionIfItsNeverUsed_ThisIsTheLazynessImTalkingAbout()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var c = sut.Connection;
            }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustNotHaveHappened();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void UsingAConnectionCreatesAnActualConnection()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var c = sut.Connection.DbConnection;
            }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void AskingForALazyTransactionDoesntDoAnythingEither()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var c = sut.Transaction;
            }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustNotHaveHappened();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void UsingATransactionCreatesBothATransactionAndAConnection()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var c = sut.Transaction.DbTransaction;
                Assert.AreSame(sut.Transaction.DbConnection, sut.Connection.DbConnection);
            }

            A.CallTo(() => _connectionFactory.CreateOpenConnection()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _transactionFactory.OpenTransaction(A<IDbConnection>.That.IsSameAs(_connection))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void TestingSomeDisposing()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var c = sut.Transaction.DbTransaction;
            }

            A.CallTo(() => _connection.Dispose()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _transaction.Dispose()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void OneTransactionPerUnitOfWorkWeSaid()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                using (var t1 = sut.Transaction)
                {
                    var doSomething = t1.DbTransaction;
                }

                ITransaction t2;
                Assert.Throws<InvalidOperationException>(() => t2 = sut.Transaction);
            }
        }


        [Test]
        public void SomeoneElseShouldBeAbleToUseOurConnectionThough()
        {
            UnitOfWork sut;
            using (sut = new UnitOfWork(_transactionFactory, _connectionFactory))
            {
                var doSomething = sut.Connection.DbConnection;

                using (var t1 = sut.Transaction)
                {
                    var doSomething1 = t1.DbTransaction;
                }

                doSomething = sut.Connection.DbConnection;
                A.CallTo(() => _connection.Dispose()).MustNotHaveHappened();
            }

            A.CallTo(() => _connection.Dispose()).MustHaveHappenedOnceExactly();
        }
    }
}