using System;
using System.Collections.Generic;

namespace dz_48
{
    public class Car
    {
        private List<Detail> _details = new List<Detail>();

        public Car(List<Detail> details)
        {
            _details = details;
        }

        public List<Detail> Details => new List<Detail>(_details);

        public void AddDetail(Detail detail)
        {
            if (detail != null)
                _details.Add(detail);
        }

        public bool TryRemoveDetail(string partName)
        {
            if (partName == null)
                return false;

            foreach (Detail part in _details)
            {
                if (part.PartName == partName)
                {
                    _details.Remove(part);
                    return true;
                }
            }

            return false;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Машина состоит из следующих деталей:\n");

            foreach (Detail detail in _details)
            {
                string state = detail.IsWorking == true ? "нармально" : "               <<<сламатая>>>";
                Console.WriteLine($" {detail.PartName} - {state}");
            }
        }
    }
}
