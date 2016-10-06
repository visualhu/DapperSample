using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IList<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product Save(Product product)
        {
            if (product.Id > 0)
            {
                _productRepository.Update(product);
            }
            else
            {
                _productRepository.Insert(product);
            }

            return product;
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }
    }
}