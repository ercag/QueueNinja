namespace QueueNinja.Domain.Dto
{
    public class JobHistoryDto
    {
        public string State { get; set; }  // e.g., "Processing", "Failed", "Succeeded"
        public DateTime CreatedAt { get; set; }
        public string ErrorMessage { get; set; }  // Null unless job failed
    }
}
