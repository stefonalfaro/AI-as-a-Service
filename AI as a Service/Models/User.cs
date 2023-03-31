namespace AI_as_a_Service.Models
{
    public class User
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public string passwordHash { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public bool? MFA { get; set; }
        public List<string> notes { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime lastModified { get; set; }
    }

    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
