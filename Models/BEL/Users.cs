using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDusingApi.Models.BEL
{
	public class Users
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int Account { get; set; }
		public int Balance { get; set; }
	}
}