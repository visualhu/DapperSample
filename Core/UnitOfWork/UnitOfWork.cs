using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContext _context;

        public UnitOfWork(IContext context)
        {
            _context = context;
            _context.BeginTransaction();
        }

        public void SaveChanges()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("Transaction has been alread committed or disposed.");

            _context.Commit();
        }

        public void Dispose()
        {
            if (_context.IsTransactionStarted)
            {
                _context.Rollback();
            }
        }
    }
}