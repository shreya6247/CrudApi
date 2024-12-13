using CRUDusingApi.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRUDusingApi.Models.BEL;


namespace CRUDusingApi.Controllers
{
    public class UserController : ApiController
    {
       private DBFunction obj = new DBFunction();
        [HttpGet]
        public IHttpActionResult getAllUsers()
        {
            List<Users> lst = obj.GetAllUsers();
            if(lst.Count == 0)
            {
                return NotFound();
            }
            return Ok(lst);
        }

        [HttpGet]
		public IHttpActionResult getUserById(int id)
		{
			Users user = obj.GetUserById(id);
			if (user==null)
			{
				return NotFound();
			}
			return Ok(user);
		}

        [HttpPatch]
		public IHttpActionResult UpdateById(int id, [FromBody] Users user)
        {
            Users existuser = obj.GetUserById(id);
            if(existuser == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(user.Username))
            {
				existuser.Username = user.Username;
			}
			if (!string.IsNullOrEmpty(user.Name))
			{
				existuser.Name = user.Name;
			}
			if (!string.IsNullOrEmpty(user.Balance.ToString()))
			{
				existuser.Balance = user.Balance;
			}
			if (!string.IsNullOrEmpty(user.Account.ToString()))
			{
				existuser.Account = user.Account;
			}
			if (!string.IsNullOrEmpty(user.Password))
			{
				existuser.Password = user.Password;
			}
			int res = obj.PostUpdate(existuser, id);
			if (res == 1)
			{
				return Ok("update successfull");
			}
			else
			{
				return BadRequest("Update unsuccessfull");
			}

		}

		[HttpPost]
        public IHttpActionResult InsertUsers([FromBody]Users user)
        {
            string str = obj.PostInsert(user);
            if(str == "Successfully inserted")
            {
				return Ok(str);
			}
            else
            {
                return BadRequest(str);
            }

        }

        [HttpPut]
        public IHttpActionResult UpdateUser(int id, [FromBody] Users user)
        {
            int res = obj.PostUpdate(user,id);
            if (res == 1)
            {
                return Ok("update successfull");
            }
            else
            {
                return BadRequest("Id does not exist");
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            int res = obj.Delete(id);
            if(res == 1)
            {
                return Ok("Delete successfull");
            }
            else
            {
                var msg = "User not found";
                var resp = Request.CreateErrorResponse(HttpStatusCode.BadRequest, msg);
				return ResponseMessage(resp);
			}
        }
    }
}
