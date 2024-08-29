namespace dz_48
{
    public class ProductFactory
    {
        public Product Create()
        {
            string[] carPartNames =
            [
                    "двигатель","кузов", "кпп", "руль", "педаль газа", "педаль тормоза", "бензобак", "безонасос",
                    "П.П. колесо", "П.Л. колесо", "З.П.колесо", "З.Л. колесо", "П. зеркало", "Л. зеркало",
                    "дворники лобового стекла", "дворники заднего стекла","Л. фара","П. фара","магнитола",
                    "антэна","амортизаторы"
            ];

            int randomIndex = Assistant.GenerateRandomNumber(carPartNames.Length -1);

            int minPrice = 1000;
            int maxPrice = 5000;
            int price = Assistant.GenerateRandomNumber(minPrice, maxPrice + 1);

            return new Product(carPartNames[randomIndex], price);
        }
    }
}
