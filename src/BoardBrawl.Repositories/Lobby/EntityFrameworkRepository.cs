﻿using BoardBrawl.Core.AutoMapping;
using BoardBrawl.Data.Application;
using BoardBrawl.Repositories.Lobby.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardBrawl.Repositories.Lobby
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public EntityFrameworkRepository(IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public void CreateGame(GameInfo gameInfo)
        {
            if (!_applicationDbContext.Games.Any(i => i.Id == gameInfo.Id))
            {
                var gameEntity = _mapper.Map<Data.Application.Models.Game>(gameInfo);

                _applicationDbContext.Games.Add(gameEntity);
                _applicationDbContext.SaveChanges();

                gameInfo.Id = gameEntity.Id;
            }
        }

        public void DeleteGame(int gameId)
        {
            var gameEntity = _applicationDbContext.Games.FirstOrDefault(i => i.Id == gameId);
            if (gameEntity != null)
            {
                _applicationDbContext.Games.Remove(gameEntity);
                _applicationDbContext.SaveChanges();
            }
        }

        public List<GameInfo> GetGames()
        {
            var games = new List<GameInfo>();

            foreach (var gameEntity in _applicationDbContext.Games.Include(i => i.Players))
            {
                var game = _mapper.Map<GameInfo>(gameEntity);
                game.PlayerCount = gameEntity.Players == null ? 0 : gameEntity.Players.Count;
                games.Add(game);
            }

            return games;
        }
    }
}