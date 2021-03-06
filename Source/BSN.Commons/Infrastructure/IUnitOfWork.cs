﻿using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void AddToQueue(ITaskUnit task);
    }
}
