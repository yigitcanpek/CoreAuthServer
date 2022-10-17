using CoreAuthServer.Core.DTO_s;
using CoreSharedLibary.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Core.Services
{
    public interface IUserService
    {

        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        
        Task<Response<UserAppDto>>GetUserByNameAsync(string userName);

    }
}
