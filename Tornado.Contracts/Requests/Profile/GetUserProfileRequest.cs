using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Contracts.Requests.Profile
{
    public record GetUserProfileRequest
    {
        public required Guid UserId { get; set; }
    }
}
