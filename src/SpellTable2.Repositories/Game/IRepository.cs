using SpellTable2.Repositories.Game.Models;

namespace SpellTable2.Repositories.Game
{
    public interface IRepository
    {
        GameInfo? GetGameInfo(Guid id);
        void CloseGame(GameInfo gameInfo);
        void AddPlayerToGame(Guid gameId, PlayerInfo playerInfo);            
        List<PlayerInfo> GetPlayers(Guid gameId);
        void UpdatePeerId(Guid gameId, Guid userId, Guid peerId);
        void DecreaseLifeTotal(Guid gameId, Guid userId, int amount);
        void IncreaseLifeTotal(Guid gameId, Guid userId, int amount);
    }
}