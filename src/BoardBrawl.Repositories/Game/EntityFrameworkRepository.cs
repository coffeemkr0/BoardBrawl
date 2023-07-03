﻿using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application;
using BoardBrawl.Data.Application.Models;
using BoardBrawl.Repositories.Game.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Repositories.Game
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public EntityFrameworkRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public void AddPlayerToGame(int gameId, PlayerInfo playerInfo)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.UserId == playerInfo.UserId);

            if (playerEntity == null)
            {
                playerEntity = _mapper.Map<Data.Application.Models.Player>(playerInfo);
                _applicationDbContext.Players.Add(playerEntity);
            }

            playerEntity.GameId = gameId;
            _applicationDbContext.SaveChanges();

            playerInfo.Id = playerEntity.Id;
            playerInfo.GameId = gameId;
        }

        public void CloseGame(GameInfo gameInfo)
        {
            var gameEntity = _applicationDbContext.Games.FirstOrDefault(i => i.Id == gameInfo.Id);

            if (gameEntity != null)
            {
                _applicationDbContext.Games.Remove(gameEntity);
                _applicationDbContext.SaveChanges();
            }
        }

        public GameInfo GetGameInfo(int id)
        {
            var gameEntity = _applicationDbContext.Games.Include(i => i.Players).First(i => i.Id == id);

            return _mapper.Map<GameInfo>(gameEntity);
        }

        public List<PlayerInfo> GetPlayers(int gameId)
        {
            var playerEntities = _applicationDbContext.Players.Where(i => i.GameId == gameId);

            return _mapper.Map<List<PlayerInfo>>(playerEntities);
        }

        public void DecreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.InfectDamage -= amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void IncreaseInfectDamage(int gameId, string userId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.GameId == gameId && i.UserId == userId);

            if (playerEntity != null)
            {
                playerEntity.InfectDamage += amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void UpdateGameInfo(GameInfo gameInfo)
        {
            var gameInfoEntity = _applicationDbContext.Games.First(i => i.Id == gameInfo.Id);

            _mapper.Map(gameInfo, gameInfoEntity);

            _applicationDbContext.SaveChanges();
        }

        public void UpdateFocusedPlayer(int playerId, int focusedPlayerId)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.Id == playerId);

            if (playerEntity != null)
            {
                playerEntity.FocusedPlayerId = focusedPlayerId;
                _applicationDbContext.SaveChanges();
            }
        }

        public PlayerInfo? GetPlayer(string userId)
        {
            var playerInfoEntity = _applicationDbContext.Players.FirstOrDefault(i => i.UserId == userId);

            if (playerInfoEntity != null)
            {
                return _mapper.Map<PlayerInfo>(playerInfoEntity);
            }

            return null;
        }

        public void UpdatePlayerInfo(PlayerInfo playerInfo)
        {
            var playerEntity = _applicationDbContext.Players.FirstOrDefault(i => i.Id == playerInfo.Id);

            if (playerEntity == null)
            {
                playerEntity = new Player();
            }

            _mapper.Map(playerInfo, playerEntity);
            _applicationDbContext.SaveChanges();
        }

        public PlayerInfo AdjustLifeTotal(int playerId, int amount)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);

            playerEntity.LifeTotal += amount;
            _applicationDbContext.SaveChanges();

            return _mapper.Map<PlayerInfo>(playerEntity);
        }

        public void UpdatePeerId(int playerId, Guid peerId)
        {
            var playerEntity = _applicationDbContext.Players.First(i => i.Id == playerId);

            playerEntity.PeerId = peerId;
            _applicationDbContext.SaveChanges();
        }
    }
}