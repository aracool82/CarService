using System;
using System.Collections.Generic;

namespace dz_48
{
    public class CarService
    {
        private const int FineForRefused = 2000;
        private const int Fine = 500;

        private int _money;
        private string[] _carPartNames;

        private Queue<Car> _cars = new Queue<Car>();
        private List<Product> _partsWarehous = new List<Product>();

        private DetailFactory _detailFactory = new DetailFactory();
        private CarFactory _carFactory = new CarFactory();
        private ProductFactory _productFactory = new ProductFactory();

        public CarService()
        {
            int carCount = 10;
            _money = 0;

            InitPartsNames();
            CreateCarQueue(carCount);
            CreatePartsWarehous();
        }

        public void Work()
        {
            const string CommandServeCar = "1";
            const string CommandRefuseService = "2";
            const string CommandExit = "3";


            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Меню  -<Автосервиса>- \n\n" +
                                  $"В очереди {_cars.Count} машин.\n" +
                                  $"Баланас - [{_money}] р.\n\n" +
                                  $"[{CommandServeCar}] - Обслужить машину.\n" +
                                  $"[{CommandRefuseService}] - Отказать в обслуживании.\n" +
                                  $"[{CommandExit}] - Выход из программы.\n");

                switch (Console.ReadLine())
                {
                    case CommandServeCar:
                        ServeCar();
                        break;

                    case CommandRefuseService:
                        RefuseService();
                        break;

                    case CommandExit:
                        isWork = false;
                        break;
                }

                Console.ReadKey();
            }
        }

        private void ServeCar()
        {
            Console.Clear();

            if (_cars.Count == 0)
            {
                Console.WriteLine("Нет машин для обслуживания. Сварачивайте свою лавочку.");
                return;
            }

            Car car = _cars.Dequeue();

            Console.WriteLine("Начальная информация автомобиля :");
            car.ShowInfo();
            Console.ReadKey();
            Console.Clear();

            if (TryFindFaultyParstInCar(car, out List<Detail> details))
            {
                Console.WriteLine("Найдены следующие неисправности:\n");

                for (int i = 0; i < details.Count; i++)
                    Console.WriteLine($"-[{details[i].PartName}]-");

                Console.WriteLine("\nПопробуем починить. Надо порыскать по складу...\n");

                for (int i = 0; i < details.Count; i++)
                {
                    if (TryFindPartInWarehousByPartName(details[i].PartName, out Product product))
                    {
                        Console.WriteLine($"{i + 1}.\nНа складе нашлась деталь [{product.PartName}] для замены.    <<стоимость>> - {product.Price} р.\n\n");

                        if (GetAnswer() == (int)Answer.Replace)
                        {
                            if (TryReplacePart(car, details[i].PartName))
                            {
                                _partsWarehous.Remove(product);
                                _money += product.Price;
                                Console.WriteLine($"Деталь [{product.PartName}] была успешно замененo(а)\n");
                            }
                            else
                            {
                                Console.WriteLine("Что то пошло не так...Не удалось заменить запчасть");
                            }
                        }
                        else 
                        {
                            AcceptFine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"На складе нет [{details[i].PartName}]");
                        AcceptFine();
                    }
                }

                Console.Clear();
                Console.WriteLine("Конечная информачия о машине :\n");
                car.ShowInfo();
                Console.ReadKey();
            }
        }

        private void AcceptFine()
        {
            Console.WriteLine($"Был выписан штраф в размере {Fine} р.");
            _money -= Fine;
        }

        private int GetAnswer()
        {
            int answer;

            Console.Write($"[{(int)Answer.Replace}] Заменить деталь?\n" +
                          $"[{(int)Answer.Cancel}] Отказатся (получить штраф {Fine})\n" +
                          $"Введите номер : ");

            bool isAnswer = false;

            do
            {
                answer = Assistant.ReadInt();

                if (answer == (int)Answer.Replace || answer == (int)Answer.Cancel)
                    isAnswer=true;
            }
            while (isAnswer = false);

            return answer;
        }

        private void RefuseService()
        {
            if (_cars.Count > 0)
            {
                Console.Clear();
                Console.WriteLine($"За отказ в обслуживаниии вы получаете фиксированный штраф в размере {FineForRefused} р.");
                _money -= FineForRefused;
                _cars.Dequeue();
            }
            else
            {
                Console.WriteLine("Всех обслужили, закрывай лавочку...");
            }
        }

        private bool TryReplacePart(Car car, string partName)
        {
            if (car == null || partName == null)
                return false;

            if (car.TryRemoveDetail(partName))
            {
                car.AddDetail(_detailFactory.Create(partName));
                return true;
            }

            return false;
        }

        private bool TryFindFaultyParstInCar(Car car, out List<Detail> parts)
        {
            parts = null;

            if (car == null)
                return false;

            if (car.Details.Count == 0)
                return false;

            List<Detail> findedFaultyParts = new List<Detail>();
            parts = car.Details;

            foreach (var part in parts)
            {
                if (part.IsWorking == false)
                    findedFaultyParts.Add(part);
            }

            if (findedFaultyParts.Count > 0)
            {
                parts = findedFaultyParts;
                return true;
            }

            return false;
        }

        private bool TryFindPartInWarehousByPartName(string partName, out Product product)
        {
            product = null;

            if (_partsWarehous == null || partName == null)
                return false;

            if (_partsWarehous.Count == 0)
                return false;

            foreach (var part in _partsWarehous)
            {
                if (part.PartName == partName)
                {
                    product = part;
                    return true;
                }
            }

            return false;
        }

        private void CreatePartsWarehous()
        {
            int completeSets = 2;

            List<Product> products = new List<Product>();

            for (int i = 0; i < _carPartNames.Length; i++)
            {
                int minPrice = 1000;
                int maxPrice = 5000;
                int price = Assistant.GenerateRandomNumber(minPrice, maxPrice + 1);
                products.Add(_productFactory.Create(_carPartNames[i], price));
            }

            for (int i = 0; i < completeSets; i++)
                _partsWarehous.AddRange(products);
        }

        private void CreateCarQueue(int carCount)
        {
            for (int i = 0; i < carCount; i++)
            {
                List<Detail> details = GetCompleteDetails();
                BreakRandomParts(details);
                _cars.Enqueue(_carFactory.Create(details));
            }
        }

        private List<Detail> GetCompleteDetails()
        {
            List<Detail> details = new List<Detail>();

            foreach (var part in _carPartNames)
                details.Add(_detailFactory.Create(part));

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

        private void InitPartsNames()
        {
            _carPartNames =
                [
                    "двигатель","кузов", "кпп", "руль", "педаль газа", "педаль тормоза", "бензобак", "безонасос",
                    "П.П. колесо", "П.Л. колесо", "З.П.колесо", "З.Л. колесо", "П. зеркало", "Л. зеркало",
                    "дворники лобового стекла", "дворники заднего стекла","Л. фара","П. фара"
                ];
        }
    }
}
