namespace dz_48
{
    public class DetailFactory
    {
        public Detail Create(string partName)
        {
            return new Detail(partName);
        }
    }
}
