using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.DTO
{
	public class UsersDto
	{
		//public int userid { get; set; }
		public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
		public string? Role { get; set; }
		//public IList<UserEvent> UserEvents { get; set; }


	}
}