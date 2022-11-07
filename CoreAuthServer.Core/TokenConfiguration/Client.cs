using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Core.TokenConfiguration
{
    public class Client
    {
        public string Id { get; set; }

        public string Secret { get; set; }

        //www.myapi.com,www.mayapii.com
        public List<string> Audiences { get; set; }


    }
}
