using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
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
                Exception = _executTask.Exception ?? new Exception("Execute Faulted");

            if (Exception != null)
                throw Exception;

            return _executTask;
        }

        public Task Rollback()
        {
            _rollbackTask.RunSynchronously();

            if (_executTask.Status == TaskStatus.Faulted)
                Exception = _executTask.Exception ?? new Exception("Rollback Faulted");

            return _rollbackTask;
        }

        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                Execute();
                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                Exception = ex;
                Rollback();
                preparingEnlistment.ForceRollback(ex);
            }
        }

        public void Rollback(Enlistment enlistment)
        {
            Rollback();
        }
    }
}
