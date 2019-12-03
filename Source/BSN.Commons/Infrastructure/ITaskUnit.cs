using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public interface ITaskUnit: IEnlistmentNotification
    {
        Exception _exception { get; set; }
        Task Execute();
        Task Rollback();
    }
}