namespace QueueNinja.Domain.Dto
{
    public class JobDto
    {
        public int Id { get; set; }
        public string InvocationData { get; set; }  // Stores job arguments
        public string State { get; set; }  // Job status (Succeeded, Failed, Processing)
        public DateTime CreatedAt { get; set; }
    }
}
