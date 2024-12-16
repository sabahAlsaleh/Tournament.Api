namespace Service.Contracts
{
    public interface IServiceManager
    {
        ITournamentService TournamentService { get; }
        IGameService GameService { get; }
        IAuthService AuthService { get; }
    }
}