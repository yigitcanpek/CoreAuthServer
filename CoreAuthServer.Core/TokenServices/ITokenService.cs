using CoreAuthServer.Core.DTO_s;
using CoreAuthServer.Core.Entities;
using CoreAuthServer.Core.TokenConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Core.TokenServices
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
