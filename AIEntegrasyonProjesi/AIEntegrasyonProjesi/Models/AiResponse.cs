namespace AiEntegrasyonProjesi.Models
{
    public class AiResponse
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string ResponseText { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}