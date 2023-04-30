using AutoMapper;

namespace SpellTable2.Services.Game
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Repositories.Game.Models.GameInfo, Models.GameInfo>();
        }
    }
}
