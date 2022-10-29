using CoreAuthServer.Core.DTO_s;
using CoreSharedLibary.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateToken(SignDto signDto);

        Task<Response<TokenDto>> CreateByRefreshToken(string refreshToken);

        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
