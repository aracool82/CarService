using System.Collections.Generic;

namespace dz_48
{
    public class CarFactory
    {
       private DetailFactory _detailFactory = new DetailFactory();

        public Car Create()
        { 
            return new Car(GetCompleteDetails());
        }

        private List<Detail> GetCompleteDetails()
        {
            string[] carPartNames =
            [
                    "двигатель","кузов", "кпп", "руль", "педаль газа", "педаль тормоза", "бензобак", "безонасос",
                    "П.П. колесо", "П.Л. колесо", "З.П.колесо", "З.Л. колесо", "П. зеркало", "Л. зеркало",
                    "дворники лобового стекла", "дворники заднего стекла","Л. фара","П. фара"
            ];

            List<Detail> details = new List<Detail>();

            foreach (var part in carPartNames)
                details.Add(_detailFactory.Create(part));

            BreakRandomParts(details);
            return details;
        }

        private void BreakRandomParts(List<Detail> details)
        {
            int maxBreakCount = 3;
            int breakCount = Assistant.GenerateRandomNumber(1, maxBreakCount + 1);

            while (breakCount > 0)
            {
                int randomIndex = Assistant.GenerateRandomNumber(details.Count);

                if (details[randomIndex].IsWorking == true)
                {
                    details[randomIndex].BrackDetail();
                    breakCount--;
                }
            }
        }
    }
}
