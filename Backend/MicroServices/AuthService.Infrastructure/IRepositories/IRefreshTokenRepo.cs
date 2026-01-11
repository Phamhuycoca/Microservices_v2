using AuthService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.IRepositories
{
    public interface IRefreshTokenRepo:IRepositories<refresh_token>
    {
    }
}
