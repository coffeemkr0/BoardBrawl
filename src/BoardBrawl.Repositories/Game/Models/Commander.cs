using BoardBrawl.Data.Application.Models;

namespace BoardBrawl.Repositories.Game.Models
{
    public class Commander
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUri { get; set; }

        public string Colors { get; set; }
    }
}