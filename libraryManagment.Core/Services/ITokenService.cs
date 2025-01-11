using libraryManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryManagment.Core.Services
{
    public interface ITokenService
    {

        public Task<string> GetTokenAsync(ApplicationUser user);

    }
}
