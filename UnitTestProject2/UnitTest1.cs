using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2
{
	[TestClass]
	public class UnitTest1
	{
		static SqlConnection connection = null;
		static SqlDataReader reader = null;
		static SqlParameter parameter = null;
		static SqlCommand command = null;

		[TestMethod]
		public void SPCustomerWithValidParamsReturnsRows()
		{
			//Arrange
			string connectionString = GetConnectionString();
			using (connection = new SqlConnection(connectionString))
			{
				// Create the command and set its properties.
				command = new SqlCommand("Customer", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				AddParameter("John", "@FirstName");
				AddParameter("Beaver", "@LastName");
				connection.Open();

				//Act
				reader = command.ExecuteReader();

				//Assert
				Assert.AreEqual(reader.HasRows, true);
			}

			CloseSQLConnection();

		}

		[TestMethod]
		public void SPCustomerWithValidParamsReturnsCorrectData()
		{
			//Arrange
			string connectionString = GetConnectionString();
			using (connection = new SqlConnection(connectionString))
			{
				// Create the command and set its properties.
				command = new SqlCommand("Customer", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				AddParameter("John", "@FirstName");
				AddParameter("Beaver", "@LastName");
				connection.Open();

				//Act
				reader = command.ExecuteReader();

				//Assert
				while (reader.HasRows)
				{
					Console.WriteLine("\t{0}\t{1}", reader.GetName(0),
						reader.GetName(1));

					while (reader.Read())
					{
						//Expected should be a param rather than hardcoded
						Assert.AreEqual(reader.GetString(0), "John");
						Assert.AreEqual(reader.GetString(1), "Beaver");
					}
					reader.NextResult();
				}

			}

			CloseSQLConnection();

		}

		[TestMethod]
		public void SPCustomerWithInvalidParamsReturns0Rows()
		{
			//Arrange
			string connectionString = GetConnectionString();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				// Create the command and set its properties.
				command = new SqlCommand("Customer", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				AddParameter("John", "@FirstName");
				AddParameter("Smith", "@LastName");
				connection.Open();

				//Act
				reader = command.ExecuteReader();

				//Assert
				Assert.AreEqual(reader.HasRows, false);
			}

			CloseSQLConnection();

		}

		[TestMethod]
		[ExpectedException(typeof(SqlException))]
		public void SPCustomerWithNoParamsThrowsException()
		{
			//Arrange
			string connectionString = GetConnectionString();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				// Create the command and set its properties.
				command = new SqlCommand("Customer", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				connection.Open();

				//Act
				reader = command.ExecuteReader();

			}

			CloseSQLConnection();
		}


		static private string GetConnectionString()
		{
			return "Data Source=(local);Initial Catalog=AdventureWorksLT;"
				+ "Integrated Security=SSPI;";
		}

		private static void AddParameter(string inputParam, string sqlParam)
		{
			parameter = new SqlParameter();
			parameter.ParameterName = sqlParam;
			parameter.SqlDbType = System.Data.SqlDbType.NVarChar;
			parameter.Direction = System.Data.ParameterDirection.Input;
			parameter.Value = inputParam;

			// Add the parameter to the Parameters collection
			if (command != null)
			{
				command.Parameters.Add(parameter);
			}
		}

		private static void CloseSQLConnection()
		{
			if (connection != null)
			{
				connection.Close();
				connection.Dispose();
			}
		}

		}
	}

