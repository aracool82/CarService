namespace dz_48
{
    public class ProductFactory
    {
        public Product Create(string partName, int price)
        { 
            return new Product(partName, price);
        }
    }
}
