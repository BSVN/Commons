using BSN.Commons.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Tests
{
    public abstract class UnitOfWorkTestBase
    {
        public abstract UnitOfWorkArrangementsAbstractFactory ArrangemenetAbstractFactoryMethod();

        [SetUp]
        public void SetUp()
        {
            UnitOfWorkArrangementsAbstractFactory abstractFactory = ArrangemenetAbstractFactoryMethod();

            _databaseFactory = abstractFactory.CreateDatabaseFactory();
            _userRepository = abstractFactory.CreateUserRepository(_databaseFactory);
            _asyncuserRepository = abstractFactory.CreateAsyncUserRepository(_databaseFactory);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseFactory.Dispose();
        }

        [Test]
        public void AddUserToDataBaseAndNullQueue_CorrectInput_UsereShouldBeCorectlyAddedToDatabase()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            _userRepository.Add(User);
            unitOfWork.Commit();

            Assert.That(_userRepository.GetMany(x => x.FirstName == "Reza"), Is.Not.Empty);
        }

        [Test]
        public void AddTaskAndDataForDatabase_CorrectInput_UsereShouldBeAddedToDatabaseAndTaskShouldBeRunCorectly()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var enlistTask = new EnlistTask
                (
                    new Task(() => Names.Add("Gholi")),
                    new Task(() => Names.Remove("Gholi"))
                );

            var secondEnlistTask = new EnlistTask
                (
                    new Task(() => Names.Add("Qamar")),
                    new Task(() => Names.Remove("Qamar"))
                );

            unitOfWork.AddToQueue(enlistTask);
            unitOfWork.AddToQueue(secondEnlistTask);
            unitOfWork.Commit();

            Assert.That(Names.Where(P => P == "Gholi").FirstOrDefault(), Is.EqualTo("Gholi"));
            Assert.That(Names.Where(P => P == "Qamar").FirstOrDefault(), Is.EqualTo("Qamar"));
        }

        [Test]
        public void AddTaskAndNullDatabase_ExceptionInjectedInput_AllTasksShouldBeRollback()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var enlistTask = new EnlistTask
                (
                    new Task(() => { Names.Add("Qamar"); }),
                    new Task(() => Names.Remove("Qamar"))
                );

            unitOfWork.AddToQueue(enlistTask);

            var enlistTask2 = new EnlistTask(
                new Task(() =>
                {
                    Names.Add("Gholi");
                    throw new Exception();
                }), new Task(() => Names.Remove("Gholi")));

            unitOfWork.AddToQueue(enlistTask2);

            try
            {
                unitOfWork.Commit();
                Assert.Fail();
            }
            catch
            {
                Assert.That(Names.Where(P => P == "Gholi").FirstOrDefault(), Is.Null);
                Assert.That(Names.Where(P => P == "Qamar").FirstOrDefault(), Is.Null);
            }
        }

        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInTask_AllTasksAndDatabaseShouldBeRollback()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            var addUser = new EnlistTask
            (
                new Task(() => { _userRepository.Add(User); }),
                new Task(() => { _userRepository.Delete(User); })
            );

            unitOfWork.AddToQueue(addUser);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var addName = new EnlistTask
            (
                new Task(() => { Names.Add("Qamar"); throw new Exception(); }),
                new Task(() => Names.Remove("Qamar"))
            );

            unitOfWork.AddToQueue(addName);

            try
            {
                unitOfWork.Commit();
                Assert.Fail();
            }
            catch
            {
                Assert.That(_userRepository.GetMany(x => x.FirstName == "Reza"), Is.Empty);
                Assert.That(Names.Where(P => P == "Gholi").FirstOrDefault(), Is.Null);
            }
        }



        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInTaskAndIncorectInput_AllTasksAndDatabaseShouldBeRollback()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
            };

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var enlistTask = new EnlistTask
            (
                new Task(() =>
                {
                    _userRepository.Add(User);
                }),
                new Task(() =>
                {
                    _userRepository.Delete(User);
                })
            );

            unitOfWork.AddToQueue(enlistTask);

            var exceptionTask = new EnlistTask
            (
                new Task(() => throw new Exception(nameof(Exception))),
                new Task(() => { })
            );

            unitOfWork.AddToQueue(exceptionTask);

            try
            {
                unitOfWork.Commit();
                Assert.Fail();
            }
            catch
            {
                Assert.That(Names.Where(P => P == "Gholi").FirstOrDefault(), Is.Null);
                Assert.That(_userRepository.GetMany(x => x.FirstName == "Reza"), Is.Empty);
            }
        }

        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInDataBase_AllTasksAndDatabaseShouldBeRollback()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User User = new User()
            {
                FirstName = "AliiReza",
                LastName = "Alizadeh",
                Password = "123456",
            };

            _userRepository.Add(User);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var enlistTask = new EnlistTask
            (
                new Task(() => { Names.Add("Qamar"); }),
                new Task(() => Names.Remove("Qamar"))
            );

            unitOfWork.AddToQueue(enlistTask);

            // TODO: Fix this test

            try
            {
                unitOfWork.Commit();
            }
            catch
            {
                Assert.That(Names.Where(P => P == "Gholi").FirstOrDefault(), Is.Null);
                Assert.That(_userRepository.GetMany(x => x.FirstName == "AliiReza"), Is.Empty);
            }
        }

        [Test]
        public void AddUserToDataBaseAndNullQueue_IncorrectInput_UsereShouldBeAddedToDatabase()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User User = new User()
            {
                FirstName = "hamidReza",
                LastName = "Alizadeh",
                Password = "123456"
            };

            try
            {
                _userRepository.Add(User);
                unitOfWork.Commit();
            }
            catch
            {
                Assert.That(_userRepository.GetMany(x => x.FirstName == "hamidReza"), Is.Empty);
            }
        }

        [Test]
        public void NoTaskForQueueAndNoDataForDataBase_CorrectInput_ShouldHaveCorrectOutput()
        {
            IUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            List<string> Names = new List<string>();

            try
            {
                unitOfWork.Commit();
            }
            catch
            {
                Assert.Fail();
            }
            Assert.That(Names, Is.Empty);
        }

        [Test]
        public async Task AddUserToDataBaseAndNullQueue_CorrectInput_UserShouldBeCorrectlyAddedToDatabaseAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var user = new User
            {
                FirstName = "AliReza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document
                {
                    Title = "AsyncTest"
                }
            };

            await _asyncuserRepository.AddAsync(user);

            await unitOfWork.CommitAsync();

            var result = await _asyncuserRepository.GetAsync(
                x => x.FirstName == "AliReza");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("AliReza"));
            Assert.That(result.Document, Is.Not.Null);
            Assert.That(result.Document.Title, Is.EqualTo("AsyncTest"));
        }

        [Test]
        public async Task AddTaskAndDataForDatabase_CorrectInput_AllTasksShouldBeExecutedAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var names = new List<string>
            {
                "Reza",
                "MohammadReza"
            };

            var enlistTask = new EnlistTask
            (
                new Task(() => names.Add("Gholi")),
                new Task(() => names.Remove("Gholi"))
            );

            var secondEnlistTask = new EnlistTask
            (
                new Task(() => names.Add("Qamar")),
                new Task(() => names.Remove("Qamar"))
            );

            unitOfWork.AddToQueue(enlistTask);
            unitOfWork.AddToQueue(secondEnlistTask);

            await unitOfWork.CommitAsync();

            Assert.That(names, Does.Contain("Gholi"));
            Assert.That(names, Does.Contain("Qamar"));
        }

        [Test]
        public async Task AddTaskAndNullDatabase_ExceptionInjectedInput_AllTasksShouldBeRolledBackAsync()
        {
            IAsyncUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            List<string> names = new List<string>
    {
        "Reza",
        "MohammadReza"
    };

            var enlistTask = new EnlistTask
            (
                new Task(() =>
                {
                    names.Add("Qamar");
                }),
                new Task(() =>
                {
                    names.Remove("Qamar");
                })
            );

            unitOfWork.AddToQueue(enlistTask);

            var enlistTask2 = new EnlistTask
            (
                new Task(() =>
                {
                    names.Add("Gholi");
                    throw new Exception("Injected exception.");
                }),
                new Task(() =>
                {
                    names.Remove("Gholi");
                })
            );

            unitOfWork.AddToQueue(enlistTask2);

            Assert.ThrowsAsync<TransactionAbortedException>(
                async () => await unitOfWork.CommitAsync());

            // All task changes must be rolled back.
            Assert.That(names.Contains("Qamar"), Is.False);
            Assert.That(names.Contains("Gholi"), Is.False);

            // The original task exception must be collected.
            Assert.That(unitOfWork.Exceptions, Has.Count.EqualTo(1));

            Assert.That(
                unitOfWork.Exceptions.Single().Message,
                Is.EqualTo("Injected exception."));
        }

        [Test]
        public async Task AddTaskAndDataForDatabase_ExceptionInjectedInTask_AllTasksAndDatabaseShouldBeRolledBackAsync()
        {
            IAsyncUnitOfWork unitOfWork = new UnitOfWork(_databaseFactory);

            User user = new User
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document
                {
                    Title = "Test"
                }
            };

            var addUser = new EnlistTask
            (
                new Task(() => _userRepository.Add(user)),
                new Task(() => _userRepository.Delete(user))
            );

            unitOfWork.AddToQueue(addUser);

            List<string> names = new List<string>
            {
                "Reza",
                "MohammadReza"
            };

            var addName = new EnlistTask
            (
                new Task(() =>
                {
                    names.Add("Qamar");
                    throw new Exception("Injected exception.");
                }),
                new Task(() => names.Remove("Qamar"))
            );

            unitOfWork.AddToQueue(addName);

            // Transaction must be aborted.
            Assert.ThrowsAsync<TransactionAbortedException>(
                async () => await unitOfWork.CommitAsync());

            // Database changes must be rolled back.
            Assert.That(
                _userRepository.GetMany(x => x.FirstName == "Reza"),
                Is.Empty);

            // Task compensation must be executed.
            Assert.That(
                names.Contains("Qamar"),
                Is.False);

            // UnitOfWork must collect the exception raised by the task.
            Assert.That(unitOfWork.Exceptions, Is.Not.Empty);

            Assert.That(
                unitOfWork.Exceptions.Any(
                    x => x.Message == "Injected exception."),
                Is.True);
        }

        [Test]
        public async Task NoTaskForQueueAndNoDataForDatabase_CommitAsyncShouldCompleteSuccessfullyAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            Assert.DoesNotThrowAsync(
                async () => await unitOfWork.CommitAsync());
        }

        [Test]
        public async Task CommitAsync_CancelledToken_ShouldThrowOperationCanceledExceptionAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            using (var cancellationTokenSource =
                new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(
                    async () =>
                        await unitOfWork.CommitAsync(
                            cancellationTokenSource.Token));

                Assert.That(exception, Is.Not.Null);
            }
        }

        [Test]
        public async Task CommitAsync_CancelledToken_UnitOfWorkShouldRemainUsableAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            using (var cancellationTokenSource =
                new CancellationTokenSource())
            {
                cancellationTokenSource.Cancel();

                Assert.CatchAsync<OperationCanceledException>(
                    async () =>
                        await unitOfWork.CommitAsync(
                            cancellationTokenSource.Token));
            }

            Assert.DoesNotThrowAsync(
                async () => await unitOfWork.CommitAsync());
        }

        [Test]
        public void AddToQueue_AfterDispose_ShouldThrowObjectDisposedException()
        {
            IUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            unitOfWork.Dispose();

            var task = new EnlistTask(
                new Task(() => { }),
                new Task(() => { }));

            Assert.Throws<ObjectDisposedException>(
                () => unitOfWork.AddToQueue(task));
        }

        [Test]
        public void Commit_AfterDispose_ShouldThrowObjectDisposedException()
        {
            IUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            unitOfWork.Dispose();

            Assert.Throws<ObjectDisposedException>(
                () => unitOfWork.Commit());
        }

        [Test]
        public void CommitAsync_AfterDispose_ShouldThrowObjectDisposedException()
        {
            IUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            unitOfWork.Dispose();

            Assert.Throws<ObjectDisposedException>(
                () => unitOfWork.Commit());
        }

        [Test]
        public void Dispose_MultipleTimes_ShouldNotThrow()
        {
            IUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            Assert.DoesNotThrow(() =>
            {
                unitOfWork.Dispose();
                unitOfWork.Dispose();
                unitOfWork.Dispose();
            });
        }

        [Test]
        public async Task Exceptions_AfterFailedCommitAsync_ShouldContainTaskExceptionAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var exception = new Exception("Test exception.");

            unitOfWork.AddToQueue(
                new EnlistTask(
                    new Task(() => throw exception),
                    new Task(() => { })));

            Assert.CatchAsync<TransactionAbortedException>(
                async () => await unitOfWork.CommitAsync());

            Assert.That(unitOfWork.Exceptions, Has.Count.EqualTo(1));

            var collectedException = unitOfWork.Exceptions.Single();

            Assert.That(collectedException, Is.SameAs(exception));
            Assert.That(collectedException.Message, Is.EqualTo("Test exception."));
        }

        [Test]
        public async Task CommitAsync_ConcurrentCalls_ShouldNotExecuteConcurrentlyAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var commits = Enumerable
                .Range(0, 10)
                .Select(_ => unitOfWork.CommitAsync())
                .ToArray();

            Assert.DoesNotThrowAsync(
                async () => await Task.WhenAll(commits));
        }

        [Test]
        public async Task AddToQueue_ConcurrentCalls_AllTasksShouldBeQueuedAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var names = new List<string>();
            var tasks = new List<ITaskUnit>();

            for (int i = 0; i < 100; i++)
            {
                var name = $"Name-{i}";

                tasks.Add(
                    new EnlistTask(
                        new Task(() => names.Add(name)),
                        new Task(() => names.Remove(name))));
            }

            await Task.WhenAll(
                tasks.Select(task =>
                    Task.Run(() => unitOfWork.AddToQueue(task))));

            await unitOfWork.CommitAsync();

            Assert.That(names.Count, Is.EqualTo(100));
        }

        [Test]
        public async Task CommitAndCommitAsync_ConcurrentCalls_ShouldBeSerializedAsync()
        {
            IAsyncUnitOfWork asyncUnitOfWork =
                new UnitOfWork(_databaseFactory);

            var names = new List<string>();

            var task = new EnlistTask
            (
                new Task(() => names.Add("Test")),
                new Task(() => names.Remove("Test"))
            );

            asyncUnitOfWork.AddToQueue(task);

            var syncUnitOfWork =
                (IUnitOfWork)asyncUnitOfWork;

            var asyncCommit = asyncUnitOfWork.CommitAsync();

            await Task.Run(() => syncUnitOfWork.Commit());

            await asyncCommit;

            Assert.That(names, Does.Contain("Test"));
        }

        [Test]
        public async Task CommitAsync_AfterSuccessfulCommit_ShouldNotExecuteTaskAgainAsync()
        {
            IAsyncUnitOfWork unitOfWork =
                new UnitOfWork(_databaseFactory);

            var executionCount = 0;

            var task = new EnlistTask
            (
                new Task(() => Interlocked.Increment(ref executionCount)),
                new Task(() => Interlocked.Decrement(ref executionCount))
            );

            unitOfWork.AddToQueue(task);

            await unitOfWork.CommitAsync();

            Assert.That(executionCount, Is.EqualTo(1));

            await unitOfWork.CommitAsync();

            Assert.That(executionCount, Is.EqualTo(1));
        }
        protected IRepository<User> _userRepository;
        protected IAsyncRepository<User> _asyncuserRepository;
        protected IDatabaseFactory _databaseFactory;
    }

}
