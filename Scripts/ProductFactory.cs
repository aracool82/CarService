namespace dz_48
{
    public class ProductFactory
    {
        public Product Create(string partName)
        {
            int minPrice = 1000;
            int maxPrice = 5000;
            int price = Assistant.GenerateRandomNumber(minPrice, maxPrice + 1);
            
            return new Product(partName, price);
        }
    }
}
