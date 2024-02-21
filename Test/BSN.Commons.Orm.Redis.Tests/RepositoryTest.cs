using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSN.Commons.Orm.Redis.Tests.Infrastructure;
using BSN.Commons.Orm.Redis.Tests.Mock;
using BSN.Commons.Orm.Redis.Tests.Dto;
using BSN.Commons.Infrastructure;
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

        [Test]
        public void AddUserToDataBaseAndNullQueue_CorrectInput_UsereShouldBeCorectlyAddedToDatabase()
        {
            User User = new User()
            {
                FirstName = "Reza",
                LastName = "Alizadeh",
                Password = "123456",
                Document = new Document() { Title = "Test" }
            };

            string userId = _userRepository.Insert(User);
            Assert.That(_userRepository.FindById(userId), Is.Not.Null);
            Assert.That(_userRepository.GetMany(x => x.FirstName == "Reza" && x.LastName == "Alizadeh"), Is.Not.Empty);
        }

        public IRedisDatabaseFactory CreateDatabaseFactory()
        {
            var redisConnectionOptions = new RedisConnectionOptions("redis://localhost:6379");
            return new DatabaseFactory(Options.Create(redisConnectionOptions));
        }

        public IRedisRepository<User> CreateUserRepository(IRedisDatabaseFactory databaseFactory)
        {
            return new UserRepository(databaseFactory);
        }
        protected IRedisRepository<User> _userRepository;
        protected IRedisDatabaseFactory _databaseFactory;
    }
}
