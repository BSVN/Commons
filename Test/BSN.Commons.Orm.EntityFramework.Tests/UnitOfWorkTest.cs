using BSN.Commons.Test.Infrastructure;
using BSN.Commons.Infrastructure;
using NUnit.Framework;
using BSN.Commons.Test.Data;
using System.Collections.Generic;
using BSN.Commons.Test.Mock;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace BSN.Commons.Test
{
    public class UnitOfWorkTest
    {
        [Test]
        public void AddUserToDataBaseAndNullQueue_CorrectInput_UsereShouldBeCorectlyAddedToDatabase()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            usersRepository.Add(User);
            unitOfWork.Commit();

            Assert.IsNotEmpty(usersRepository.GetMany(x => x.FirstName == "Reza"));
        }

        [Test]
        public void AddTaskAndDataForDatabase_CorrectInput_UsereShouldBeAddedToDatabaseAndTaskShouldBeRunCorectly()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

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

            Assert.AreEqual(Names.Where(P => P == "Gholi").FirstOrDefault(), "Gholi");
            Assert.AreEqual(Names.Where(P => P == "Qamar").FirstOrDefault(), "Qamar");
        }

        [Test]
        public void AddTaskAndNullDatabase_ExceptionInjectedInput_AllTasksShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

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
                Assert.NotNull(null);
            }
            catch (Exception)
            {
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
                Assert.IsNull(Names.Where(P => P == "Qamar").FirstOrDefault());
            }
        }

        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInTask_AllTasksAndDatabaseShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            var addUser = new EnlistTask
            (
                new Task(() => { usersRepository.Add(User); }),
                new Task(() => { usersRepository.Delete(User); })
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
                Assert.NotNull(null);
            }
            catch (Exception)
            {
                Assert.IsEmpty(usersRepository.GetMany(x => x.FirstName == "Reza"));
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
            }
        }



        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInTaskAndIncorectInput_AllTasksAndDatabaseShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

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
                    usersRepository.Add(User);
                }),
                new Task(() =>
                {
                    usersRepository.Delete(User);
                })
            );

            unitOfWork.AddToQueue(enlistTask);

            var exceptionTask = new EnlistTask
            (
                new Task(() => throw new Exception(nameof(Exception))),
                new Task(() => { })
            );

            unitOfWork.AddToQueue(enlistTask);

            try
            {
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
                Assert.IsEmpty(usersRepository.GetMany(x => x.FirstName == "Reza"));
            }
        }

        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInDataBase_AllTasksAndDatabaseShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

            User User = new User()
            {
                FirstName = "AliiReza",
                LastName = "Alizadeh",
                Password = "123456",
            };

            usersRepository.Add(User);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            var enlistTask = new EnlistTask
            (
                new Task(() => { Names.Add("Qamar"); }),
                new Task(() => Names.Remove("Qamar"))
            );

            unitOfWork.AddToQueue(enlistTask);

            try
            {
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
                Assert.IsEmpty(usersRepository.GetMany(x => x.FirstName == "AliiReza"));
            }
        }

        [Test]
        public void AddUserToDataBaseAndNullQueue_IncorrectInput_UsereShouldBeAddedToDatabase()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

            User User = new User()
            {
                FirstName = "hamidReza",
                LastName = "Alizadeh",
                Password = "123456"
            };

            try
            {
                usersRepository.Add(User);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Assert.IsEmpty(usersRepository.GetMany(x => x.FirstName == "hamidReza"));
            }
        }

        [Test]
        public void NoTaskForQueueAndNoDataForDataBase_CorrectInput_ShouldHaveCorrectOutput()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var usersRepository = new UserRepository(databaseFactory);

            List<string> Names = new List<string>();

            try
            {
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Assert.IsFalse(false);
            }
            Assert.IsEmpty(Names);
        }
    }
}
