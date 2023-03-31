namespace AI_as_a_Service.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }

        public string status { get; set; }
        public bool isRestricted { get; set; }
        public int? planId { get; set; }
        public List<string>? notes { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModified { get; set; }
    }
}
