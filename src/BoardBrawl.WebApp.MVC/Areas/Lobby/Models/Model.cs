namespace BoardBrawl.WebApp.MVC.Areas.Lobby.Models
{
    public class Model
    {
        public string UserId { get; set; }
        public List<GameInfo> MyGames { get; set; }

        public Model()
        {
            MyGames = new List<GameInfo>();
        }
    }
}