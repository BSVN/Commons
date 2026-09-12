using BSN.Commons.Infrastructure;
using BSN.Commons.Orm.Redis.Tests.Dto;
using BSN.Commons.Orm.Redis.Tests.Mock;
using BSN.Commons.Test.Infrastructure;
using NUnit.Framework;
using Testcontainers.Redis;

namespace BSN.Commons.Orm.Redis.Tests
{
    [TestFixture]
    public class RepositoryTest
    {

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _redis = new RedisBuilder("redis/redis-stack-server:latest")
                .Build();

            await _redis.StartAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _redis.DisposeAsync();
        }

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
            return new InMemoryDatabaseFactory(_redis);
        }

        public IRepository<User> CreateUserRepository(IDatabaseFactory databaseFactory)
        {
            return new UserRepository(databaseFactory);
        }

        private RedisContainer _redis = null!;
        protected IRepository<User> _userRepository;
        protected IDatabaseFactory _databaseFactory;
    }
}
