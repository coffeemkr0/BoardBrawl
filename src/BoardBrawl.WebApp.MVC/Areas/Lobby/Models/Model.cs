namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Models
{
    public class Model
    {
        public Guid UserId { get; set; }
        public List<GameInfo> MyGames { get; set; }

        public Model()
        {
            MyGames = new List<GameInfo>();
        }
    }
}