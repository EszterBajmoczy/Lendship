using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly LendshipDbContext _dbContext;

        public EvaluationRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(EvaluationAdvertiser evaluation)
        {
            _dbContext.EvaluationAdvertisers.Add(evaluation);
            _dbContext.SaveChanges();
        }

        public void Create(EvaluationLender evaluation)
        {
            _dbContext.EvaluationLenders.Add(evaluation);
            _dbContext.SaveChanges();
        }

        public IEnumerable<EvaluationAdvertiser> GetAdvertiserByUser(string userId)
        {
            return _dbContext.EvaluationAdvertisers
                        .Include(e => e.UserFrom)
                        .Include(e => e.Advertisement)
                        .Where(e => e.UserTo.Id == userId);
        }

        public IEnumerable<EvaluationLender> GetLenderByUser(string userId)
        {
            return _dbContext.EvaluationLenders
                        .Include(e => e.UserFrom)
                        .Include(e => e.Advertisement)
                        .Where(e => e.UserTo.Id == userId);
        }

        public IEnumerable<EvaluationAdvertiser> GetAdvertiserEvaluationsByUser(string userId)
        {
            return _dbContext.EvaluationAdvertisers
                        .Include(e => e.UserTo)
                        .Where(e => e.UserTo.Id == userId);
        }

        public IEnumerable<EvaluationLender> GetLenderEvaluationsByUser(string userId)
        {
            return _dbContext.EvaluationLenders
                        .Include(e => e.UserTo)
                        .Where(e => e.UserTo.Id == userId);
        }
    }
}
