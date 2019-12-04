using System;
using System.Linq;
using Commons.Infrastructure;
using NUnit.Framework;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSN.Commons.Test
{
    public class UnitOfWorkTest
    {
        [Test]
        public void AddUserToDataBaseAndNullQueue_CorrectInput_UsereShouldBeCorectlyAddedToDatabase()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var Context = databaseFactory.Get();
            var Users = Context.Set<User>();

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            Users.Add(User);
            unitOfWork.Commit();

            Assert.AreEqual(User.FirstName, Users.First().FirstName);
            Assert.AreEqual(User.LastName, Users.First().LastName);
            Assert.AreEqual(User.Password, Users.First().Password);
        }

        [Test]
        public void AddUserToDataBaseAndNullQueue_IncorrectInput_UsereShouldBeAddedToDatabase()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var Context = databaseFactory.Get();
            var Users = Context.Set<User>();

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456"
            };

            Users.Add(User);

            System.Action action = () => unitOfWork.Commit();
            Assert.Throws<DbUpdateException>(new TestDelegate(action));
        }

        [Test]
        public void AddTaskAndDataForDatabase_CorrectInput_UsereShouldBeAddedToDatabaseAndTaskShouldBeRunCorectly()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            unitOfWork.AddToQueue(new Task(() => Names.Add("Gholi")), new Task(() => Names.Remove("Gholi")));
            unitOfWork.Commit();

            Assert.AreEqual(Names.Where(P => P == "Gholi").FirstOrDefault(), "Gholi");
        }

        [Test]
        public void AddTaskAndNullDatabase_ExceptionInjectedInput_AllTasksShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            unitOfWork.AddToQueue(new Task(() =>
            {
                Names.Add("Qamar");
            }), new Task(() => Names.Remove("Qamar")));

            unitOfWork.AddToQueue(new Task(() =>
            {
                Names.Add("Gholi");
                throw new Exception();
            }), new Task(() => Names.Remove("Gholi")));

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

            var Context = databaseFactory.Get();
            var Users = Context.Set<User>();

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            Users.Add(User);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            unitOfWork.AddToQueue(new Task(() =>
            {
                Names.Add("Gholi");
                throw new Exception();
            }), new Task(() => Names.Remove("Gholi")));

            try
            {
                unitOfWork.Commit();
                Assert.NotNull(null);
            }
            catch (Exception)
            {
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
                Assert.AreEqual(Users.Count(), 0);
            }
        }

        [Test]
        public void AddTaskAndDataForDatabase_ExceptionInjectedInTaskAndIncorectInput_AllTasksAndDatabaseShouldBeRollback()
        {
            var databaseFactory = new InMemoryDatabaseFactory();
            IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);

            var Context = databaseFactory.Get();
            var Users = Context.Set<User>();

            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
            };

            Users.Add(User);

            List<string> Names = new List<string>() { "Reza", "MohammadReza" };

            unitOfWork.AddToQueue(new Task(() =>
            {
                Names.Add("Gholi");
                throw new Exception();
            }), new Task(() => Names.Remove("Gholi")));

            try
            {
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                Assert.IsNull(Names.Where(P => P == "Gholi").FirstOrDefault());
                Assert.AreEqual(Users.Count(), 0);
            }
        }
    }
}
