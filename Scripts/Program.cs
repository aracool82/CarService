using System.Collections.Generic;

namespace dz_48
{
    public class Program
    {
        private static void Main()
        {
            CarFactory carFactory = new CarFactory();
            Queue<Car> cars = new Queue<Car>();
            int carCount = 10;

            for (int i = 0; i < carCount; i++)
                cars.Enqueue(carFactory.Create());
            
            CarService carService = new CarService(cars);
            carService.Work();
        }
    }
}
