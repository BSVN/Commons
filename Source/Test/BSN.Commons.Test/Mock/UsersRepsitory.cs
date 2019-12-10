using BSN.Commons.Test.Data;
using Commons.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSN.Commons.Test.Mock
{
    public sealed class UserRepository : RepositoryBase<User>, IRepository<User>
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        { }
    }
}
