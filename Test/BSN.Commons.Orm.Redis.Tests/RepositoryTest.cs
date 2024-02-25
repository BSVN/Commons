using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSN.Commons.Orm.Redis.Tests.Mock;
using BSN.Commons.Orm.Redis.Tests.Dto;
using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace BSN.Commons.Orm.Redis.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        [SetUp]
        public void SetUp()
        {
            _databaseFactory = CreateDatabaseFactory();
            _userRepository = CreateUserRepository(_databaseFactory);
        }

        [TearDown]
        public void TearDown()
        {
            _databaseFactory.Dispose();
        }

        [Test]
        public void AddUserToDataBase_UserShouldBeCorrectlyAddedToDatabase()
        {
            User user = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            _userRepository.Add(user);

            Assert.That(_userRepository.GetById<string>(user.Id.ToString()), Is.Not.Null);
            Assert.That(_userRepository.GetMany(x => x.FirstName == "Reza" && x.LastName == "Alizadeh"), Is.Not.Empty);
        }

        public IDatabaseFactory CreateDatabaseFactory()
        {
            var redisConnectionOptions = new RedisConnectionOptions
            {
                ConnectionString = "redis://localhost:6379"
            };

            var dbContext = new RedisDbContext(Options.Create(redisConnectionOptions));

            return new RedisDatabaseFactory(dbContext);
        }

        public IRepository<User> CreateUserRepository(IDatabaseFactory databaseFactory)
        {
            return new UserRepository(databaseFactory);
        }

        protected IRepository<User> _userRepository;
        protected IDatabaseFactory _databaseFactory;
    }
}
