using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LendshipDbContext _dbContext;

        public UserRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(ApplicationUser user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public ApplicationUser GetById(string id)
        {
            return _dbContext.Users
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public void Update(ApplicationUser user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void DeleteById(string userId)
        {
            var user = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public IEnumerable<string> GetIdsByEmails(List<string> emails)
        {
            return _dbContext.Users
                        .Where(u => emails.Contains(u.Email))
                        .Select(u => u.Id);
        }

        public ApplicationUser GetByEmail(string email)
        {
            return _dbContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefault();
        }
    }
}
