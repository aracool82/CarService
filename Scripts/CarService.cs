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
            {
                Console.WriteLine("У машины дефектов НЕТ");
                return;
            }    

            Console.WriteLine("Найдены следующие неисправности:\n");

            for (int i = 0; i < faultyDetails.Count; i++)
                Console.WriteLine($"-[{faultyDetails[i].PartName}]-");

            Console.WriteLine("\nПопробуем починить. Надо порыскать по складу...\n" +
                "Найденые товары на складе :\n");

            List<string> partNamesForReplace = new List<string>();
            int fineCounter = 0;

            for (int i = 0; i < faultyDetails.Count; i++)
            {
                if (_warehouse.TryFindProduct(faultyDetails[i].PartName, out Product findedPart))
                {
                    partNamesForReplace.Add(findedPart.PartName);
                    Console.WriteLine($"[{findedPart.PartName}] - наличие на складе [ДА] - цена замены - [{findedPart.Price}].");
                }
                else
                {
                    fineCounter++;
                    Console.WriteLine($"[{faultyDetails[i].PartName}] - наличие на складе [Нет] .");
                    Console.WriteLine("Вынуждены отказать в ремонте...\n");
                }
            }

            if (partNamesForReplace.Count > 0)
            {
                for (int i = 0; i < partNamesForReplace.Count; i++)
                {
                    if (GetAnswer(partNamesForReplace[i]) == Answer.Replace)
                    {
                        car.ReplaceDetail(_detailFactory.Create(partNamesForReplace[i]));
                        _money += _warehouse.GetProductPrice(partNamesForReplace[i]);
                        _warehouse.RemoveProductByName(partNamesForReplace[i]);
                        Console.WriteLine($"Деталь [{partNamesForReplace[i]}] была успешно заменена(о)\n");
                    }
                    else
                    {
                        fineCounter++;
                    }
                }
            }

            if (fineCounter > 0)
            {
                _money -= fineCounter * Fine;
                Console.WriteLine($"\nЗа невыполненые ремонты или отказы нам впаяли штраф {fineCounter * Fine}");
                Console.ReadKey();
            }
            else 
            {
                PrintCarInfo("Машина полностью здорова...\nКонечная информачия о машине :", car);
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

        private Answer GetAnswer(string partName)
        {
            int number;

            Console.Write($"\n---[{partName}]--- :\n\n" +
                          $"[{(int)Answer.Replace}] Заменить деталь?\n" +
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
