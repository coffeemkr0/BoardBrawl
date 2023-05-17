namespace BoardBrawl.Core.AutoMapping
{
    public interface IMapper
    {
        TDestination Map<TDestination>(object source);
    }
}
