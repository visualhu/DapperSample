using Data.Contexts;
using Data.Entities;
using Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Application
{
    public class ProductApp : IProductApp
    {
        private readonly IProductService _productService;
        private readonly UnitOfWorkFactory _uowFactory;

        public ProductApp(IProductService productService, UnitOfWorkFactory uowFactory)
        {
            _productService = productService;
            _uowFactory = uowFactory;
        }

        public IList<Product> GetAll()
        {
            return _productService.GetAll();
        }

        public Product Save(Product product)
        {
            try
            {
                using (var uow = _uowFactory.Create())
                {
                    _productService.Save(product);
                    uow.SaveChanges();
                }
                return product;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Product GetById(int id)
        {
            return _productService.GetById(id);
        }
    }
}