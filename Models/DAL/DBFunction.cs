using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using CRUDusingApi.Models.BEL;

namespace CRUDusingApi.Models.DAL
{
	public class DBFunction
	{
		private string str = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

		
		public List<Users> GetAllUsers()
		{
			List<Users> users = new List<Users>();

			using (SqlConnection conn = new SqlConnection(str))
			{
				SqlCommand cmd = new SqlCommand("SELECT * FROM Users", conn);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					Users user = new Users
					{
						Id = Convert.ToInt32(reader["Id"]),
						Name = reader["Name"].ToString(),
						Username = reader["Username"].ToString(),
						Password = reader["Password"].ToString(),
						Account = Convert.ToInt32(reader["AccNo"]),
						Balance = Convert.ToInt32(reader["Balance"])
					};

					users.Add(user);
				}
			}

			return users;
		}

	
		public string PostInsert(Users user)
		{
			using (SqlConnection conn = new SqlConnection(str))
			{
				SqlCommand cmd = new SqlCommand("sp_insert_user", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@name", user.Name);
				cmd.Parameters.AddWithValue("@username", user.Username);
				cmd.Parameters.AddWithValue("@password", user.Password);
				cmd.Parameters.AddWithValue("@acc_no", user.Account);
				cmd.Parameters.AddWithValue("@balance", user.Balance);

				
				SqlParameter parameter = new SqlParameter()
				{
					ParameterName = "@msg",
					SqlDbType = SqlDbType.VarChar, 
					Size = 100, 
					Direction = ParameterDirection.Output
				};

				cmd.Parameters.Add(parameter);

				conn.Open();
				int res = cmd.ExecuteNonQuery();

				conn.Close();
				return parameter.Value.ToString();
			}
		}


		public int PostUpdate(Users user, int id)
		{

			using (SqlConnection conn = new SqlConnection(this.str))
			{
				SqlCommand cmd = new SqlCommand("update Users set Name=@name,Username=@username,Password = @password,AccNo=@acc_no,Balance=@balance where Id=@id", conn);

				cmd.Parameters.AddWithValue("@id", id);
				cmd.Parameters.AddWithValue("@name", user.Name);
				cmd.Parameters.AddWithValue("@username", user.Username);
				cmd.Parameters.AddWithValue("@password", user.Password);
				cmd.Parameters.AddWithValue("@acc_no", user.Account);
				cmd.Parameters.AddWithValue("@balance", user.Balance);


				conn.Open();
				int res = cmd.ExecuteNonQuery();
				if (res > 0)
				{
					return 1;
				}
				else
				{
					return 0;
				}

			}
		}

			public int Delete(int id)
		{

			using (SqlConnection conn = new SqlConnection(str))
			{
				SqlCommand cmd = new SqlCommand("delete from Users where Id=@id", conn);
				cmd.Parameters.AddWithValue("@id", id);

				conn.Open();
				int res = cmd.ExecuteNonQuery();
				if (res > 0)
				{
					return 1;
				}
				else
				{
					return 0;
				}

			}
		}


		public Users GetUserById(int id)
		{

			using (SqlConnection conn = new SqlConnection(str))
			{
				SqlCommand cmd = new SqlCommand("SELECT * FROM Users where Id=@id", conn);
				cmd.Parameters.AddWithValue("@id", id);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				Users user = new Users();
				while (reader.Read())
				{
					user.Id = Convert.ToInt32(reader["Id"]);
					user.Name = reader["Name"].ToString();
					user.Username = reader["Username"].ToString();
					user.Password = reader["Password"].ToString();
					user.Account = Convert.ToInt32(reader["AccNo"]);
					user.Balance = Convert.ToInt32(reader["Balance"]);
				}
				return user;
			}
		}
	}
}
