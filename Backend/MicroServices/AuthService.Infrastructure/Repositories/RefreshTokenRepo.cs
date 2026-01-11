using AuthService.Domain.Entites;
using AuthService.Infrastructure.AppContext;
using AuthService.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepo : Repositories<refresh_token>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
