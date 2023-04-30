using AutoMapper;

namespace SpellTable2.Services.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.GameInfo, Repositories.Lobby.Models.GameInfo>();
            CreateMap<Repositories.Lobby.Models.GameInfo, Models.GameInfo>();
        }
    }
}
