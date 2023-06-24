using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Services.Game.Models
{
    public class Commander
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUri { get; set; }

        public List<Colors> Colors { get; set; }

        public Commander()
        {
            Colors = new List<Colors>();
        }
    }
}