using System;
using Bunker.Business.Entities;
using Bunker.Business.Entities.Dictioneries;
using Bunker.Business.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class InitLoaderService : IInitLoaderService
    {
        private readonly BunkerDbContext _dbContext;

        public InitLoaderService(BunkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Init()
        {
            //TODO fill db
            var values = Enum.GetValues(typeof(RoleDictionary));

            // foreach (var VARIABLE in COLLECTION)
            // {
            //     
            // }
        }
    }
}