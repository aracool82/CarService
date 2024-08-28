namespace dz_48
{
    public class Detail
    {
        public Detail(string partName)
        {
            PartName = partName;
            IsWorking = true;
        }

        public string PartName { get; private set; }
        public bool IsWorking { get; private set; }

        public void BrackDetail()
        { 
            IsWorking = false;
        }

        public void FixDetail()
        {
            IsWorking = true;
        }
    }
}
