using CW.DTOs.Request;
using CW.DTOs.Response;
using CW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    public class SqlServerStudentDBService : IStudentDBService
    {
        private const string ConString2 = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s17557;Integrated Security=True";
        public ResponseState EnrollStudent(EnrollStudentRequest newstudent)
        {
            var student = new Student();

            student.FirstName = newstudent.FirstName;
            student.LastName = newstudent.LastName;

            using (var con = new SqlConnection(ConString2))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                var tran = con.BeginTransaction();
                try
                {
                    //1. czy studia istnieją
                    com.CommandText = "select IdStudies from Studies where Name=@name";
                    com.Parameters.AddWithValue("name", newstudent.Studies);
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        Console.WriteLine("Takie studia nie istnieją");
                        tran.Rollback();
                        return ResponseState.NoDataChanged;
                        //return BadRequest("Studia nie istnieją");
                    }
                    int idStudies = (int)dr["IdStudy"];
                    dr.Close();

                    //2. Enrollment
                    int IdEnrollment = 5;
                    com.CommandText = "select IdEnrollment from Enrollment where Semester = @Semester and IdStudy = @IdStudies";
                    com.Parameters.AddWithValue("IdStudies", idStudies);
                    com.Parameters.AddWithValue("Semester", 1);
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Read();
                        com.CommandText = "Insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) values (@IdEnrollment, @Semester2, @IdStudy, @StartDate)";
                        com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        com.Parameters.AddWithValue("Semester2", 1);
                        com.Parameters.AddWithValue("IdStudy", idStudies);
                        com.Parameters.AddWithValue("StartDate", DateTime.Now);
                        dr.Close();
                        com.ExecuteNonQuery();

                    }
                    else
                    {
                        IdEnrollment = (int)dr["IdEnrollment"];
                        dr.Close();
                    }
                    //3. Czy nr indeksu jest unikalny
                    com.CommandText = "select * from Student where IndexNumber = @IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", newstudent.IndexNumber);
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        Console.WriteLine("Taki numer indeksu już istnieje w bazie");
                        return ResponseState.NoDataChanged;
                    }

                    //4. dodanie studenta
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                    com.Parameters.AddWithValue("IndexNumber", newstudent.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", newstudent.FirstName);
                    com.Parameters.AddWithValue("LastName", newstudent.LastName);
                    com.Parameters.AddWithValue("BirthDate", newstudent.BirthDate);
                    com.Parameters.AddWithValue("IdEnrollment", newstudent.IdEnrollment);
                    com.ExecuteNonQuery();
                    
                    tran.Commit();
                    return ResponseState.Success;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Wystąpił błąd: " + ex);
                    tran.Rollback();
                    return ResponseState.Fail;
                    //return BadRequest(ex);
                }
            }
            throw new NotImplementedException();
        }

        public Student GetStudent(string index)
        {
            var st = new Student();
            using (var con = new SqlConnection(ConString2))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                try
                {
                    com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @Index";
                    com.Parameters.AddWithValue("Index", index);
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        return null;
                    }
                    else
                    {
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                        st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wystąpił błąd: " + e);
                    return null;
                }
                return st;
            }
        }

        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }

        public ResponseState PromoteStudents(Promote_StudentRequest request)
        {
            throw new NotImplementedException();
        }

        ResponseState IStudentDBService.EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(ConString2))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                try
                {
                    com.CommandText = "select IdEnrollment from Enrollment " +
                        "where IdStudy = (SELECT IdStudy FROM Studies WHERE name = @Name) AND semester = @Semester";
                    com.Parameters.AddWithValue("Name", request.Studies);
                    com.Parameters.AddWithValue("Semester", request.Semester);
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        return ResponseState.NoDataChanged;
                    }
                    else
                    {
                        dr.Close();
                        var cmd = con.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "PromoteStudents";
                        cmd.Parameters.AddWithValue("@Studies", request.Studies);
                        cmd.Parameters.AddWithValue("@Semester", request.Semester);
                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wystąpił błąd: " + e);
                    return ResponseState.Fail;
                }
                return ResponseState.Success;
            }
        }
    }
}
