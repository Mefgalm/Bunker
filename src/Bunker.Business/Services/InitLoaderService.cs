using System;
using System.Linq;
using System.Transactions;
using Bunker.Business.Entities;
using Bunker.Business.Entities.Dictioneries;
using Bunker.Business.Extensions;
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
            //var values = Enum.GetValues(typeof(RoleDictionary)).Cast<RoleDictionary>();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var roles = _dbContext.Roles.ToList();

                    _dbContext.Roles.AddRange(Enum.GetValues(typeof(RoleDictionary))
                                                  .Cast<RoleDictionary>()
                                                  .Where(x => roles.All(q => q.Id != x.Identifier()))
                                                  .Select(x => new Role
                                                  {
                                                      Id   = x.Identifier(),
                                                      Name = x.Name()
                                                  }));

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}