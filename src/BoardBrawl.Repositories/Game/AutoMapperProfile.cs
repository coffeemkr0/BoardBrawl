﻿using AutoMapper;
using BoardBrawl.Repositories.Game.Models;

namespace BoardBrawl.Repositories.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameInfo, Data.Application.Models.Game>();
            CreateMap<Data.Application.Models.Game, GameInfo>();
            CreateMap<Data.Application.Models.CommanderDamage, CommanderDamage>();
            CreateMap<Data.Application.Models.CardHistoryEntry, CardHistoryEntry>();

            CreateMap<Data.Application.Models.Player, PlayerInfo>();
            CreateMap<PlayerInfo, Data.Application.Models.Player>();
            
        }
    }
}