using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BWWeb.Controllers
{
    public class HomeController : Controller
    {
        BWFramework.BLL.TUser bllTUser = new BWFramework.BLL.TUser();
        //
        // GET: /Home/

        /// <summary>
        /// 主页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        public ActionResult Lgoin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Lgoin(string loginNo, string password)
        {
            var tmpTUser = bllTUser.GetModelByLoginNo(loginNo);
            return View();
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        public ActionResult Register()
        {
            Session["LastUrl"] = Request.RawUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Register(string loginNo, string password)
        {
            var tmpTUser = bllTUser.GetModelByLoginNo(loginNo);
            return View();
        }

    }
}
