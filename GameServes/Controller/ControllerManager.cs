using Common;
using GameServes.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameServes.Controller
{
    internal class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            InitController();
            this.server = server;
        }
        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
        }
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data,Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (!isGet)
            {
                Console.WriteLine("未获取到" + requestCode + "所对应的Controller，无法处理请求");
                return;
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);//枚举是0，1，2数字类型，该方法时获取对应的英文是啥
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine($"[警告] 在Controller[{controller.GetType()}]中，没有对应的处理方法:[{methodName}];"); return;
            }
            object[] paramters = new object[] {data,client,server};
            object o = mi.Invoke(controller, paramters);
            if(o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(client,requestCode,o as string);


        }
    }
}
