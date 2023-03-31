namespace AI_as_a_Service.Models
{
    public class ChatCompletions
    {
        public class ChatCompletionRequest
        {
            public List<Message> Messages { get; set; }
            public int? FineTuningId { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }

        public class ChatCompletionLog
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public DateTime Timestamp { get; set; }
        }

    }
}
