using AngularRegister.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularRegister.Controllers
{
    
    public class ValuesController : ApiController
    {
		private ApplicationDbContext context = new ApplicationDbContext();
		// GET api/values
		public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

		[HttpGet]
		[Route("api/getUser/:id")]
		public IHttpActionResult get() {
			string id;
			id = User.Identity.GetUserId();
			id = RequestContext.Principal.Identity.GetUserId();
			var retval = context.Users.Select(x => new
			{ Id = x.Id, Email = x.Email, UserName = x.UserName, FirstName = x.FirstName, LastName = x.LastName, LoggedOn = DateTime.Now.ToString() });
			return Ok(retval);
		}

		
	}
}
