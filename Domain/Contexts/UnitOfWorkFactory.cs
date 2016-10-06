using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public readonly DbContext _context;

        public UnitOfWorkFactory(DbContext context)
        {
            _context = context;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_context);
        }
    }
}