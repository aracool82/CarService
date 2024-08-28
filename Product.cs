namespace dz_48
{
    public class Product 
    {
        public Product(string partName,int price)
        {
            Price = price;
            PartName = partName;
        }

        public string PartName { get; private set; }
        public int Price { get; private set; }
    }
}
