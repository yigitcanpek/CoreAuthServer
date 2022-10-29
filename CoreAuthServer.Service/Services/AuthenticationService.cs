using CoreAuthServer.Core.DTO_s;
using CoreAuthServer.Core.Entities;
using CoreAuthServer.Core.Repositories;
using CoreAuthServer.Core.Services;
using CoreAuthServer.Core.TokenConfiguration;
using CoreAuthServer.Core.TokenServices;
using CoreAuthServer.Core.UnitOfWork;
using CoreSharedLibary.DTO_s;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<Client>> optionsClient,IGenericRepository<UserRefreshToken> userRefreshTokenService, IUnitOfWork unitOfWork, ITokenService tokenService, UserManager<UserApp> userManager)
        {
            _userRefreshTokenService = userRefreshTokenService;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _clients = optionsClient.Value;
            _userManager = userManager;
        }

        public async Task<Response<TokenDto>> CreateByRefreshToken(string refreshToken)
        {
            UserRefreshToken existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken==null)
            {
                return Response<TokenDto>.Fail("Resfresh token not found", 404,
                    true);
            }
            UserApp user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user==null)
            {
                return Response<TokenDto>.Fail("User Id not found", 404, true);
            }

            TokenDto tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expration=tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<TokenDto>> CreateToken(SignDto signDto)
        {
            if (signDto == null) throw new ArgumentNullException(nameof(signDto));
            var user = await _userManager.FindByEmailAsync(signDto.Email);

            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

            if (!await _userManager.CheckPasswordAsync(user,signDto.Password))
            {
                return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            }

            TokenDto token = _tokenService.CreateToken(user);

            UserRefreshToken userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expration = token.RefreshTokenExpiration });
            }

            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);

        }


        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            Client client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }

            ClientTokenDto token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            UserRefreshToken existRefreshToken = _userRefreshTokenService.Where(x=> x.Code == refreshToken).SingleOrDefault();

            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh Token Not Found",404,true);
            }
            _userRefreshTokenService.Remove(existRefreshToken);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
