using Data.Contexts;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;

        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public IList<Product> GetAll()
        {
            return _context.Query<Product>("select * from products").ToList();
        }

        public void Insert(Product product)
        {
            _context.Execute("insert into products values(@Name,@Price)", product);
        }

        public void Update(Product product)
        {
            _context.Execute("update products set name=@Name,price=@Price", product);
        }

        public Product GetById(int id)
        {
            return _context.Query<Product>("select * from products where id=@Id", new { Id = id }).FirstOrDefault();
        }
    }
}