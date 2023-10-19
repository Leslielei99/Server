using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServes.Servers;

namespace GameServes.Controller
{
    abstract class BaseController
    {
        RequestCode requestCode = RequestCode.Nome;
        public RequestCode RequestCode { get { return requestCode; } }

        public virtual string DefaultHandle(string data,Client client,Server server)
        {
            return null;
        }
    }
}
