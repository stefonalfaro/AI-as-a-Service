namespace AI_as_a_Service.Models
{
    public class Plan
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public string term { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int maxDailyRequests { get; set; }
    }
}
