using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
{

    public class EnlistTask : ITaskUnit
    {
        private readonly Task _executeTask;
        private readonly Task _rollbackTask;

        public Exception Exception { get; set; }

        public EnlistTask(Task execute, Task rollback)
        {
            _executeTask = execute
                ?? throw new ArgumentNullException(nameof(execute));

            _rollbackTask = rollback
                ?? throw new ArgumentNullException(nameof(rollback));
        }

        public Task Execute()
        {
            try
            {
                _executeTask.RunSynchronously();

                if (_executeTask.IsFaulted)
                {
                    Exception = _executeTask.Exception?.InnerException
                        ?? new Exception("Execute task faulted.");

                    throw Exception;
                }

                if (_executeTask.IsCanceled)
                {
                    Exception = new TaskCanceledException(_executeTask);

                    throw Exception;
                }

                return _executeTask;
            }
            catch (Exception ex)
            {
                Exception = Exception ?? ex;

                throw;
            }
        }

        public Task Rollback()
        {
            try
            {
                _rollbackTask.RunSynchronously();

                if (_rollbackTask.IsFaulted)
                {
                    var rollbackException =
                        _rollbackTask.Exception?.InnerException
                        ?? new Exception("Rollback task faulted.");

                    Exception = Exception ?? rollbackException;

                    throw rollbackException;
                }

                if (_rollbackTask.IsCanceled)
                {
                    var rollbackException =
                        new TaskCanceledException(_rollbackTask);

                    Exception = Exception ?? rollbackException;

                    throw rollbackException;
                }

                return _rollbackTask;
            }
            catch (Exception ex)
            {
                Exception = Exception ?? ex;
                throw;
            }
        }

        public void Prepare(
            PreparingEnlistment preparingEnlistment)
        {
            try
            {
                Execute();

                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                Exception = Exception ?? ex;

                try
                {
                    Rollback();
                }
                catch
                {
                    // The original transaction failure must remain
                    // the failure reported to System.Transactions.
                }

                preparingEnlistment.ForceRollback(Exception);
            }
        }

        public void Commit(
            Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Rollback(
            Enlistment enlistment)
        {
            try
            {
                Rollback();
            }
            finally
            {
                enlistment.Done();
            }
        }

        public void InDoubt(
            Enlistment enlistment)
        {
            enlistment.Done();
        }
    }
}