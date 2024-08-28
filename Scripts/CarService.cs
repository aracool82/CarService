using System;
using System.Collections.Generic;

namespace dz_48
{
    public class CarService
    {
        private const int FineForRefused = 10000;
        private const int Fine = 1000;

        private int _money;
        private string[] _carPartNames;

        private Queue<Car> _cars = new Queue<Car>();
        private Warehouse _warehouse = new Warehouse();

        private DetailFactory _detailFactory = new DetailFactory();
        private CarFactory _carFactory = new CarFactory();
        private ProductFactory _productFactory = new ProductFactory();

        public CarService(Queue<Car> cars)
        {
            _cars= cars;
            _money = 0;
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
                        Serve();
                        break;

                    case CommandRefuseService:
                        Refuse();
                        break;

                    case CommandExit:
                        isWork = false;
                        break;
                }

                Console.ReadKey();
            }
        }

        private void Serve()
        {
            Console.Clear();

            if (_cars.Count == 0)
            {
                Console.WriteLine("Нет машин для обслуживания. Сварачивайте свою лавочку.");
                return;
            }

            Car car = _cars.Dequeue();
            PrintCarInfo("Начальная информация автомобиля ", car);

            if (TryFindFaultyPars(car, out List<Detail> details))
            {
                Console.WriteLine("Найдены следующие неисправности:\n");

                for (int i = 0; i < details.Count; i++)
                    Console.WriteLine($"-[{details[i].PartName}]-");

                Console.WriteLine("\nПопробуем починить. Надо порыскать по складу...\n");

                for (int i = 0; i < details.Count; i++)
                {
                    if (_warehouse.TryFindProduct(details[i].PartName, out Product product))
                    {
                        Console.WriteLine($"{i + 1}.\nНа складе нашлась деталь [{product.PartName}] для замены.    <<стоимость>> - {product.Price} р.\n\n");

                        if (GetAnswer() == (int)Answer.Replace)
                        {
                            if (TryReplacePart(car, details[i].PartName))
                            {
                                _money += product.Price;
                                Console.WriteLine($"Деталь [{product.PartName}] была успешно замененo(а)\n");

                                if (_warehouse.TryRemoveProduct(product.PartName))
                                    Console.WriteLine("И удалена со склада");
                                else
                                    Console.WriteLine("Не удалось удалить со склада продукт");
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

                PrintCarInfo("Конечная информачия о машине", car);
            }
        }

        private void PrintCarInfo(string message, Car car)
        {
            Console.Clear();
            Console.WriteLine($"{message} :\n");
            car.ShowInfo();
            Console.ReadKey();
            Console.Clear();
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
                    isAnswer = true;
            }
            while (isAnswer = false);

            return answer;
        }

        private void Refuse()
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
            if (car == null || string.IsNullOrWhiteSpace(partName))
                return false;

            if (car.TryRemoveDetail(partName))
            {
                car.AddDetail(_detailFactory.Create(partName));
                return true;
            }

            return false;
        }

        private bool TryFindFaultyPars(Car car, out List<Detail> parts)
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
    }
}
