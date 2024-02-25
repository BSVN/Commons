using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redis.OM.Modeling;

namespace BSN.Commons.Orm.Redis.Tests.Dto
{
    [Document(StorageType = StorageType.Hash, Prefixes = new[] { "User" })]
    public class User
    {
        [RedisIdField] public Ulid Id { get; set; }
        [Indexed] public string FirstName { get; set; }
        [Indexed] public string LastName { get; set; }
        [Indexed(CascadeDepth = 1)] public Document Document { get; set; }
        public string Password { get; set; }
    }
}
