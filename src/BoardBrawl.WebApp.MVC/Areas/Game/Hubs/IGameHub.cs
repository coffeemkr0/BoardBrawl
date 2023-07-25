namespace BoardBrawl.WebApp.MVC.Areas.Game.Hubs
{
    public interface IGameHub
    {
        Task OnPlayerJoined(int playerId, Guid peerId);
        Task OnPlayerDisconnected(int playerId);
        Task OnNewPeerRequested(int requestingPlayerId);
        Task OnNewPeerReceived(int playerId, string peerId);
    }
}
