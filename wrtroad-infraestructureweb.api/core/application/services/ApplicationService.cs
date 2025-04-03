using AutoMapper;
using wrtroad_infraestructureweb.api.core.infrastructure.data.context;
using wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories;

namespace wrtroad_infraestructureweb.api
{
    public partial class ApplicationService : IApplicationService
    {
        private readonly WrtRoadDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderRepository _emailSenderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationService(
            WrtRoadDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IAuthRepository authRepository,
            IEmailSenderRepository emailSenderRepository,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _authRepository = authRepository;
            _emailSenderRepository = emailSenderRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
