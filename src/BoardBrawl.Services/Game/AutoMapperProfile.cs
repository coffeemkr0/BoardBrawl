using AutoMapper;

namespace BoardBrawl.Services.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.GameInfo, Repositories.Game.Models.GameInfo>();
            CreateMap<Repositories.Game.Models.GameInfo, Models.GameInfo>();

            CreateMap<Repositories.Game.Models.PlayerInfo, Models.PlayerInfo>();
            CreateMap<Models.PlayerInfo, Repositories.Game.Models.PlayerInfo>();
        }
    }
}