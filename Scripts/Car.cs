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

        public void ReplaceDetail(Detail detail)
        {
            if (detail == null)
                return;

            if (!TryRemoveDetail(detail.PartName))
                return;

            _details.Add(detail);
        }

        private bool TryRemoveDetail(string partName)
        {
            if (string.IsNullOrWhiteSpace(partName))
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
                string state = detail.IsWorking == true ? "ok" : "\t\t <<<сламатая>>>";
                Console.WriteLine($" {detail.PartName} - {state}");
            }
        }
    }
}
