using System;
using System.Collections.Generic;

namespace dz_48
{
    public class CarService
    {
        private const int FineForRefused = 10000;
        private const int Fine = 1000;

        private int _money;

        private Queue<Car> _cars = new Queue<Car>();
        private Warehouse _warehouse = new Warehouse();

        private DetailFactory _detailFactory = new DetailFactory();

        public CarService(Queue<Car> cars)
        {
            _cars = cars;
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

            if (TryFindFaultyPars(car, out List<Detail> faultyDetails) == false)
                return;

            Console.WriteLine("Найдены следующие неисправности:\n");

            for (int i = 0; i < faultyDetails.Count; i++)
                Console.WriteLine($"-[{faultyDetails[i].PartName}]-");

            Console.WriteLine("\nПопробуем починить. Надо порыскать по складу...\n");

            for (int i = 0; i < faultyDetails.Count; i++)
            {
                if (_warehouse.TryFindProduct(faultyDetails[i].PartName, out Product findedPart))
                {
                    Console.WriteLine($"{i + 1}.\nНа складе нашлась деталь [{findedPart.PartName}] для замены.    <<стоимость>> - {findedPart.Price} р.\n\n");

                    if (GetAnswer() == Answer.Replace)
                    {
                        car.ReplaceDetail(_detailFactory.Create(findedPart.PartName));
                        _money += findedPart.Price;
                        _warehouse.RemoveProductByName(findedPart.PartName);
                        Console.WriteLine($"Деталь [{findedPart.PartName}] была успешно замененo(а)\n");
                    }
                    else
                    {
                        AcceptFine();
                    }
                }
                else
                {
                    Console.WriteLine($"На складе нет [{faultyDetails[i].PartName}]");
                    AcceptFine();
                }
            }

            PrintCarInfo("Конечная информачия о машине", car);
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

        private Answer GetAnswer()
        {
            int number;

            Console.Write($"[{(int)Answer.Replace}] Заменить деталь?\n" +
                          $"[{(int)Answer.Cancel}] Отказатся (получить штраф {Fine})\n" +
                          $"Введите номер : ");

            bool isAnswer = false;

            do
            {
                number = Assistant.ReadInt();

                if (number == (int)Answer.Replace || number == (int)Answer.Cancel)
                    isAnswer = true;
            }
            while (isAnswer == false);

            return number == (int)Answer.Replace ? Answer.Replace : Answer.Cancel;
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
