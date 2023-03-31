namespace AI_as_a_Service.Models
{
    public class Payment
    {
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvc { get; set; }
    }
}
