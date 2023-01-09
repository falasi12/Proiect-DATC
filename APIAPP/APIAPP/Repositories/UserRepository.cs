using APIAPP.Models;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace APIAPP.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InfoContext dbContext;

        public UserRepository(InfoContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> TryLogin(string username, string password)
        {
                bool resultat = false;
                await (
                                    from g in dbContext.User
                                    select g.Username == username && g.Password == password)
                                    .FirstOrDefaultAsync()
                                    .Select(result => resultat = result);

                return resultat;

        }

        public async Task<bool> TryAdminLogin(string username, string password)
        {
            bool resultat = false;
            await (
                                from g in dbContext.User
                                select g.Username == username && g.Password == password && g.Admin == true)
                                .FirstOrDefaultAsync()
                                .Select(result => resultat = result);

            return resultat;

        }
    }
}
