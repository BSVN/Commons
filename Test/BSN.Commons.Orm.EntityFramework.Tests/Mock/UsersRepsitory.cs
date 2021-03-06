﻿using BSN.Commons.Test.Data;
using BSN.Commons.Infrastructure;
using BSN.Commons.Orm.EntityFramework;
using BSN.Commons.Tests;

namespace BSN.Commons.Test.Mock
{
    public sealed class UserRepository : RepositoryBase<User>, IRepository<User>
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        { }
    }
}
