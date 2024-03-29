﻿using AutoMapper;
using BoardBrawl.Repositories.Lobby.Models;

namespace BoardBrawl.Repositories.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameInfo, Data.Application.Models.Game>();
            CreateMap<Data.Application.Models.Game, GameInfo>();

            CreateMap<Data.Application.Models.Player, PlayerInfo>();
            CreateMap<PlayerInfo, Data.Application.Models.Player>();
        }
    }
}