
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id, string userId);

        void PassTurn(int gameId);
        void JoinGame(int gameId, string userId);
        void UpdatePeerId(int playerId, Guid peerId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        void AdjustLifeTotal(int playerId, int amount);
        void AdjustInfectCount(int playerId, int amount);
        void AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount);
        void UpdateCommander(int playerId, int slot, string cardId);
    }
}