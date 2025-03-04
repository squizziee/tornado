﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Domain.Models.Auth;

namespace Tornado.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);  
        Task<User?> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);  
        Task<User?> GetWithProfileAsync(User user, CancellationToken cancellationToken);  
    }
}
