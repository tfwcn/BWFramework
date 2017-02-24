using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BWWeb.Controllers
{
    public class HomeController : Controller
    {
        BWFramework.BLL.TUser bllTUser = new BWFramework.BLL.TUser();
        BWFramework.BLL.TNo bllTNo = new BWFramework.BLL.TNo();
        //
        // GET: /Home/

        /// <summary>
        /// 主页面
        /// </summary>
        public ActionResult Index()
        {
            //return View();
            List<Thread> listThread = new List<Thread>();
            using (BWFramework.BLL.DBTransaction tran = new BWFramework.BLL.DBTransaction())
            {
                Mutex mutex = new Mutex();//进程互斥
                for (int i = 0; i < 1000; i++)
                {
                   Thread t= new Thread((tmpi) =>
                    {
                        List<BWFramework.Model.TNo> tmpListTNo = new List<BWFramework.Model.TNo>();
                        for (int i2 = 0; i2 < 100; i2++)
                        {
                            var strCID = Guid.NewGuid();
                            tmpListTNo.Add(new BWFramework.Model.TNo() { CID = strCID.ToString(), CNo = BWFramework.Common.GUIDHelper.GetNumEnNo(tmpi.ToString().PadLeft(3, '0') + i2.ToString().PadLeft(2, '0'), 4) });
                        }
                        mutex.WaitOne();
                        bllTNo.Add(tmpListTNo);
                        mutex.ReleaseMutex();
                    });
                    t.Start(i);
                    listThread.Add(t);
                }
                foreach (Thread t in listThread)
                {
                    t.Join();
                }
                tran.Commit();
            }//*/
            int te = -1123213;

            return this.Content(te + " " + ((uint)te));
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
