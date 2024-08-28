using System.Collections.Generic;

namespace dz_48
{
    public class CarFactory
    {
        public Car Create(List<Detail> details)
        { 
            return new Car(details);
        }
    }
}
