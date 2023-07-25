namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public interface IGameHub
    {
        Task OnPlayerJoined(string playerId, Guid peerId);
        Task OnPlayerDisconnected(int playerId);
    }
}
