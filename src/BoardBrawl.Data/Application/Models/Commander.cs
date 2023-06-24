namespace BoardBrawl.Data.Application.Models
{
    public class Commander
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public string Name { get; set; }

        public string ImageUri { get; set; }

        public string Colors { get; set; }

        public Player Player { get; set; }
    }
}