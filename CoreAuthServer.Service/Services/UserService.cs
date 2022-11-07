using AutoMapper.Internal.Mappers;
using CoreAuthServer.Core.DTO_s;
using CoreAuthServer.Core.Entities;
using CoreAuthServer.Core.Services;
using CoreAuthServer.Service.Mapper;
using CoreSharedLibary.DTO_s;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            UserApp user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };
           
            IdentityResult result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
        
        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            UserApp user = await _userManager.FindByNameAsync(userName);
            if (user==null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404, true);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
