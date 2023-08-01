using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select EmployeeId, EmployeeName, Department, DateOfJoining, PhotoFileName from dbo.Employee
";
            DataTable dataTable = new DataTable();
            string SqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using(SqlConnection sqlConnection = new SqlConnection(SqlDataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)) { 
                sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load(sqlDataReader);
                sqlDataReader.Close();
                sqlConnection.Close();
                }
            }return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                insert into dbo.Employee(EmployeeName, Department, DateOfJoining, PhotoFileName) values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
";
            DataTable dataTable = new DataTable();
            string SqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(SqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);

                    sqlDataReader = sqlCommand.ExecuteReader();                                
                    dataTable.Load(sqlDataReader);                                             
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Data inserted Successfully");
        }
        [HttpPut]
        public JsonResult Put(Employee employee)
        {

            string query = @"update dbo.Employee set EmployeeName= @EmployeeName, Department = @Department,
            DateOfJoining= @DateOfJoining, PhotoFileName= @PhotoFileName
                where  EmployeeId= @EmployeeId   
            ";

            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);


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

            string query = @"delete from dbo.Employee  where  EmployeeId= @EmployeeId   
            ";

            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader sqlDataReader;
            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("savefile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var path = _webHostEnvironment.ContentRootPath + "/Photos/"+ filename;


                using (var stream = new FileStream(path, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("logo.png");
            }
        }
    }
}
