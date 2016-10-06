using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Application
{
    public interface IProductApp
    {
        IList<Product> GetAll();

        Product Save(Product product);

        Product GetById(int id);
    }
}