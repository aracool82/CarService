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
            if(TryRemoveProduct(partName))
                Console.WriteLine("Продукт удален со склада");
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

        private bool TryRemoveProduct(string partName)
        {
            if (string.IsNullOrWhiteSpace(partName))
                return false;

            if (_products.Count == 0)
                return false;

            foreach (Product product in _products)
            {
                if (product.PartName == partName)
                {
                    _products.Remove(product);
                    return true;
                }
            }

            return false;
        }

        private void CreateProducts()
        {
            string[] carPartNames =
                [
                    "двигатель","кузов", "кпп", "руль", "педаль газа", "педаль тормоза", "бензобак", "безонасос",
                    "П.П. колесо", "П.Л. колесо", "З.П.колесо", "З.Л. колесо", "П. зеркало", "Л. зеркало",
                    "дворники лобового стекла", "дворники заднего стекла","Л. фара","П. фара","магнитола",
                    "антэна","амортизаторы"
                ];

            for (int i = 0; i < carPartNames.Length; i++)
            {
                int minPrice = 1000;
                int maxPrice = 5000;
                int price = Assistant.GenerateRandomNumber(minPrice, maxPrice + 1);

                int minPartCount = 2;
                int maxPartCount = 6;
                int partCount = Assistant.GenerateRandomNumber(minPartCount, maxPartCount + 1);

                for (int j = 0; j < partCount; j++)
                    _products.Add(_productFactory.Create(carPartNames[i], price));
            }
        }
    }
}
