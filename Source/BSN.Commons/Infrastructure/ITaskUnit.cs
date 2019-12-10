using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public interface ITaskUnit : IEnlistmentNotification
    {
        Exception Exception { get; set; }
        Task Execute();
        Task Rollback();
    }
}