namespace SpellTable2.Core.AutoMapping
{
    public interface IMapper
    {
        TDestination Map<TDestination>(object source);
    }
}
