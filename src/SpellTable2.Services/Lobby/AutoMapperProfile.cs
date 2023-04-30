using AutoMapper;

namespace SpellTable2.Services.Lobby
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Repositories.Lobby.Models.GameInfo, Models.GameInfo>();
        }
    }
}
