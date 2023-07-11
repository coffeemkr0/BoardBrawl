using AutoMapper;
using BoardBrawl.Services.Game.Models;

namespace BoardBrawl.Services.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameInfo, Repositories.Game.Models.GameInfo>();
            CreateMap<Repositories.Game.Models.GameInfo, GameInfo>();

            CreateMap<Repositories.Game.Models.PlayerInfo, PlayerInfo>();
            CreateMap<PlayerInfo, Repositories.Game.Models.PlayerInfo>();

            CreateMap<Repositories.Game.Models.CardHistoryEntry, CardHistoryEntry>();
        }
    }
}