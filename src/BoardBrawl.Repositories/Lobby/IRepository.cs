
using BoardBrawl.Repositories.Models;

namespace BoardBrawl.Repositories.Lobby
{
    public interface IRepository
    {
        List<GameInfo> GetGames();
        void CreateGame(GameInfo gameInfo);
        void DeleteGame(Guid gameId);
        void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo);
    }
}