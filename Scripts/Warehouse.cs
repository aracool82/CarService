using System;
using System.Collections.Generic;

namespace dz_48
{
    public class Warehouse
    {
        private List<Product> _products = new List<Product>();
        private ProductFactory _productFactory = new ProductFactory();

        public Warehouse()
        {
            CreateProducts();
        }

        public void RemoveProductByName(string partName)
        {
            if (TryFindProduct(partName, out Product findedPart))
            {
                Console.WriteLine("Продукт удален со склада");
                _products.Remove(findedPart);
            }
        }

        public bool TryFindProduct(string partName, out Product findedPart)
        {
            findedPart = null;

            if (string.IsNullOrWhiteSpace(partName))
                return false;

            if (_products.Count == 0)
                return false;

            foreach (var part in _products)
            {
                if (part.PartName == partName)
                {
                    findedPart = part;
                    return true;
                }
            }

            return false;
        }

        private void CreateProducts()
        {
            int minPartCount = 20;
            int maxPartCount = 40;
            int partCount = Assistant.GenerateRandomNumber(minPartCount, maxPartCount + 1);

            for (int j = 0; j < partCount; j++)
                _products.Add(_productFactory.Create());
        }
    }
}
