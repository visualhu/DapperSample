using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public interface IProductRepository
    {
        IList<Product> GetAll();

        void Insert(Product product);

        void Update(Product product);

        Product GetById(int id);
    }
}