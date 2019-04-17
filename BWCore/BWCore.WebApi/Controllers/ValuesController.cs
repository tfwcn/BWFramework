using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BWCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private AppSettings Config;
        private BLL.TUser bllTUser;

        public ValuesController(IOptions<AppSettings> setting)
        {
            Config = setting.Value;
            bllTUser = new BLL.TUser(Config.ConnectionString);
        }
        // GET api/values
        [HttpGet]
        public ActionResult<List<Model.TUser>> Get()
        {
            bllTUser.Add(new Model.TUser() { FId = Guid.NewGuid().ToString(), FCreateTime = DateTime.Now, FName = "testUser" });
            var list = bllTUser.GetModels();
            return list;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
