using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface ICreatable<TArgument, TResult>
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        static abstract TResult Create(TArgument argument);
    }
}
