using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Repositories.Game
{
    public interface IRepository
    {
        GameInfo? GetGameInfo(int id);
        void CloseGame(GameInfo gameInfo);
        void AddPlayerToGame(int gameId, PlayerInfo playerInfo);            
        List<PlayerInfo> GetPlayers(int gameId);
        void UpdatePeerId(int gameId, Guid userId, Guid peerId);
        void DecreaseLifeTotal(int gameId, Guid userId, int amount);
        void IncreaseLifeTotal(int gameId, Guid userId, int amount);
        void DecreaseCommanderDamage(int gameId, Guid userId, int amount);
        void IncreaseCommanderDamage(int gameId, Guid userId, int amount);
        void DecreaseInfectDamage(int gameId, Guid userId, int amount);
        void IncreaseInfectDamage(int gameId, Guid userId, int amount);
        void ClearPlayers();
    }
}