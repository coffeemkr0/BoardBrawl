using AutoMapper;

namespace BoardBrawl.Services.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.GameInfo, Repositories.Models.GameInfo>();
            CreateMap<Repositories.Models.GameInfo, Models.GameInfo>();
        }
    }
}
