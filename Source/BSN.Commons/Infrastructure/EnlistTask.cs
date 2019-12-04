using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public class EnlistTask : ITaskUnit
    {
        private readonly Task _executTask;
        private readonly Task _rollbackTask;

        public Exception Exception { get; set; }

        public EnlistTask(Task execut, Task rollback)
        {
            _executTask = execut;
            _rollbackTask = rollback;
        }

        public Task Execute()
        {
            _executTask.RunSynchronously();

            if (_executTask.Status == TaskStatus.Faulted)
                Exception = _executTask.Exception ?? new Exception("Execute");

            return _executTask;
        }

        public Task Rollback()
        {
            _rollbackTask.RunSynchronously();

            if (_executTask.Status == TaskStatus.Faulted)
                Exception = _executTask.Exception ?? new Exception("Rollback");

            return _rollbackTask;
        }
    }
}
