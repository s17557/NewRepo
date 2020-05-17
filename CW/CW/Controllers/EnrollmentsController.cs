using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CW.DTOs.Request;
using CW.DTOs.Response;
using CW.Models;
using CW.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//niezbędne dla walidacji
    public class EnrollmentsController : ControllerBase
    {
        private const string ConString2 = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s17557;Integrated Security=True";
        private IStudentDBService _service;

        
        public EnrollmentsController(IStudentDBService service)
        {
            _service = service;
        }
        

        [HttpPost]
        [Route("api/enrollments")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        //public IActionResult EnrollStudent(Student newstudent)
        {
            return RespoState(_service.EnrollStudent(request));
            //walidacja
            /*if(newstudent.FirstName==null || newstudent.LastName == null)
            {
                return BadRequest("Brak danych");
            }
            */
            /*
            using(var con=new SqlConnection(ConString2))
            using(var com=new SqlCommand())
            {
                
                com.Connection = con;
                var tran = con.BeginTransaction();
                try
                {
                    //czy studia istnieją
                    com.CommandText = "select IdStudies from studies where name=@name";
                    com.Parameters.AddWithValue("name", newstudent.Studies);
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("Studia nie istnieją");
                    }
                    //
                    
                    int idstudies = (int)dr["IdStudies"];
                    //dodanie studenta
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, Studies) VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @Studies)";
                    com.Parameters.AddWithValue("IndexNumber", newstudent.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", newstudent.FirstName);
                    com.Parameters.AddWithValue("LastName", newstudent.LastName);
                    com.Parameters.AddWithValue("BirthDate", newstudent.BirthDate);
                    com.Parameters.AddWithValue("Studies", newstudent.Studies);
                    //..
                    com.ExecuteNonQuery();
                    tran.Commit();
                }catch(SqlException ex)
                {
                    tran.Rollback();
                    return BadRequest(ex);
                }
            }*/
            return Ok(); 
        }
        [HttpPost]
        [Route("api/enrollments/promotions")]
        public IActionResult PromoteStudents(Promote_StudentRequest request)
        {
            return RespoState(_service.PromoteStudents(request));
        }
        private IActionResult RespoState(ResponseState response)
        {
            if (response == ResponseState.Success)
            {
                return Ok();
            }
            else if (response == ResponseState.NoDataChanged)
            {
                return NoContent();
            }
            else return BadRequest();
        }

    }
}