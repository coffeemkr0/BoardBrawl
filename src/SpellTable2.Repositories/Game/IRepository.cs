namespace SpellTable2.Repositories.Game
{
    public interface IRepository
    {
        string GetPlayerName();
        void SetPlayerName(string playerName);
    }
}