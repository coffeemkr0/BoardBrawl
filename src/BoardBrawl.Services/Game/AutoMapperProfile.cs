using AutoMapper;
using BoardBrawl.Repositories.Models;

namespace BoardBrawl.Services.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.GameInfo, GameInfo>();
            CreateMap<GameInfo, Models.GameInfo>();

            CreateMap<PlayerInfo, Models.PlayerInfo>();
            CreateMap<Models.PlayerInfo, PlayerInfo>();
        }
    }
}