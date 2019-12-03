using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public class EnlistTask : ITaskUnit
    {
        private readonly Task _executTask;
        private readonly Task _rollbackTask;

        public Exception _exception { get; set; }

        public EnlistTask(Task execut, Task rollback)
        {
            _executTask = execut;
            _rollbackTask = rollback;
        }


        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                var task = Execute();

                if (task.Status == TaskStatus.Faulted)
                {
                    _exception = task.Exception ?? new Exception("Prepare");
                    throw _exception;
                }

                preparingEnlistment.Done();
            }
            catch (Exception ex)
            {
                _exception = ex;
                preparingEnlistment.ForceRollback();
                throw _exception;
            }
        }

        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public Task Execute()
        {
            _executTask.RunSynchronously();
            return _executTask;
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public Task Rollback()
        {
            _rollbackTask.RunSynchronously();

            return _rollbackTask;

        }

        public void Rollback(Enlistment enlistment)
        {

            var task = Rollback();

            if (task.Status == TaskStatus.Faulted)
            {
                _exception = task.Exception ?? new Exception("Rollback");
                throw _exception;
            }
            enlistment.Done();

        }
    }
}
