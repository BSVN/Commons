using Redis.OM.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.Redis.Tests.Dto
{
    public class Document
    {
        [Indexed] public string Title { get; set; }
    }
}
