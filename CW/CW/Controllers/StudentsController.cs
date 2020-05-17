using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CW.Models;
using CW.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IDBService _dbService;
        private const string ConString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s17557;Integrated Security=True";
        //private IStudentsDel _dbService;

              public StudentsController(IDBService service)
        {
            _dbService = service;
        }

        //2. Query String
        [HttpGet]
        public IActionResult GetStudents(String orderBy) //api/students?orderBy=lastname&pageSize=11
        {
            if (orderBy == "LastName")//sortowanie po LastName
            {
                return Ok(_dbService.GetStudents().OrderBy(s => s.LastName));
            }
            return Ok(_dbService.GetStudents());
        }

        //1.
        //[HttpGet("{id:min(1):max(5}}")]
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            return Ok("A");
            //return NotFound("nie znaleziono");
        }
        //3. body postman
        [HttpPost]
        public IActionResult CreateStudent(Models.Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

            return Ok(student);
        }
        [HttpGet]
        public IActionResult GetStudent()
        {
            //2. Budowa connection string
            /*var conBuilder= new SqlConnectionStringBuilder();
             * conBuilder.InitialCatalog = "s17557";
             * ...
             * string conStr = conBuilder.ConnectionString;
             */
            var list = new List<Student>();
            //1. komunikacja nisko poziomowa z bazą danych 
            using (SqlConnection con = new SqlConnection(ConString)) //wymagany string nazwa serwera i bazy danych
            //con.Open();
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select *from students";
                con.Open();
                SqlDataReader dr = com.ExecuteReader();  //wykład 4 42min
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNymber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    list.Add(st);
                }
            }
            //con.Dispose();
            return Ok();
        }

        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudent(string IndexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select *from students where indexnumber=@index";
                com.Parameters.AddWithValue("index", IndexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();

                    //if (dr["IndexNumber"] == DBNull.Value) {} //sprawdzenie nulla bazodanowgo który jest obiektem

                    st.IndexNumber = dr["IndexNymber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    return Ok(st);
                }

            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public IActionResult GetEnrollments(string id)
        {
            return Ok(_dbService.GetEnrollments(id));
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(Student student)
        {
            return Ok("Akutalizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStduent()
        {
            return Ok("Usuwanie zakończone");
        }


        [HttpGet]
        public IActionResult GetStudent2(string IndexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "insert into Student(FirstName) values (@firstName)";

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                transaction.Commit();

                try
                {
                    int affectedRows = com.ExecuteNonQuery();

                    com.CommandText = "update into ...";
                    com.ExecuteNonQuery();
                    //...
                    transaction.Commit();
                }catch(Exception exc)
                {
                    transaction.Rollback();
                }

            }
            return Ok();
        }
    }
    
}