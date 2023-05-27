using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<Pokupatel>, IUserRepository
    {
        public UserRepository(InternetstoreContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Pokupatel?> GetByEmailAndPassword(string email, string password)
        {
            var result = await base.FindByCondition(x => x.Email == email && x.Password == password);
            if(result==null || result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
