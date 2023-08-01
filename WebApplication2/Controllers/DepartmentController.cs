using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration) {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                Select DepartmentId, DepartmentName from dbo.Department;    
            ";
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader ;
            using(SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) { 
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {

            string query = @"insert into dbo.Department values(@DepartmentName)    
            ";

            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {

            string query = @"update dbo.Department set DepartmentName= @DepartmentName where  DepartmentId= @DepartmentId   
            ";

            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {

            string query = @"delete from dbo.Department  where  DepartmentId= @DepartmentId   
            ";
             
            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
