using BSN.Commons.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            unitOfWork.AddToQueue(enlistTask);

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

        protected IRepository<User> _userRepository;
        protected IDatabaseFactory _databaseFactory;
    }

}
