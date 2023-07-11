
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public interface IService
    {
        GameInfo? GetGameInfo(int id, string userId);

        void PassTurn(int gameId);
        bool IsPlayerInGame(int gameId, string userId);
        void JoinGame(int gameId, string userId);
        void UpdatePeerId(int playerId, Guid peerId);
        void UpdateFocusedPlayer(int playerId, int focusedPlayerId);
        void AdjustLifeTotal(int playerId, int amount);
        void AdjustInfectCount(int playerId, int amount);
        void AdjustCommanderDamage(int gameId, int playerId, int ownerPlayerId, string cardId, int amount);
        void UpdateCommander(int playerId, int slot, string cardId);
        void UpdatePlayerTurnOrder(int gameId, List<int> playerIds);
        void PromoteToGameOwner(int gameId, int playerId);
        void LeadGame(int gameId, int playerId);
        void CloseGame(int gameId);
        void AddCardToCardHistory(int gameId, int playerId, string cardId);
        void RemoveCardFromCardHistory(int id);
    }
}