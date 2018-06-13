namespace Bunker.Business.Services
{
    public class BaseService
    {
        protected BunkerDbContext _dbContext;

        public BaseService(BunkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }               
    }
}