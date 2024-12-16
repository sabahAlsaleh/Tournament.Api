using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Tournament.Core.Entities;
using Tournament.Core.Repository;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
     {
        private readonly Lazy<ITournamentService> _tournamentService;
        private readonly Lazy<IGameService> _gameService;
        private readonly Lazy<IAuthService> _authService;

        //public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _tournamentService = new Lazy<ITournamentService>(() => new TournamentService(unitOfWork, mapper));
        //    _gameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
        //}
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _tournamentService = new Lazy<ITournamentService>(() => new TournamentService(unitOfWork, mapper));
            _gameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
            _authService = new Lazy<IAuthService>(() => new AuthService(mapper,userManager, roleManager ));
        }
        public ITournamentService TournamentService => _tournamentService.Value;
        public IGameService GameService => _gameService.Value;

        public IAuthService AuthService =>_authService.Value;
    }

}
