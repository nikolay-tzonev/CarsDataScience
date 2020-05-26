namespace CrawlerPoC
{
    public class Record
    {
        public string Url { get; set; }
        public Result Result { get; set; }
        public string Title { get; set; }
        public string Model { get; set; }
        public string ModelUrl { get; set; }
        public decimal Price { get; set; }
        public string Condition { get; set; }
        public string VehicleType { get; set; }
        public string Engine { get; set; }
        public string Hp { get; set; }
        public string Mileage { get; set; }
        public string Transmision { get; set; }
        public string Manufactured { get; set; }
        

    }

    public enum Condition
    {
        Used,
        New
    }

    public enum Result
    {
        OK,
        Missing,
        Error
    }
}