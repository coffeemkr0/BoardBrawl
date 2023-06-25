namespace BoardBrawl.WebApp.MVC.Areas.Game.Models
{
    public class CardInfo
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public Uri ImageUriSmall { get; set; }
        public Uri ImageUri { get; set; }
        public Uri ImageUriLarge { get; set; }
        public List<Colors> Colors { get; set; }

        public CardInfo()
        {
            Colors = new List<Colors>();
        }
    }
}