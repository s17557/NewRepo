using CW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    public class SqlServerDBDal : IStudentsDel
    {   
        private static List<Student> students;
        private static List<Enrollment> enrollments;
        private string sqlConnection = "Data Source=db-mssql;Initial Catalog=s17557;Integrated Security=True";
        public IEnumerable<Enrollment> GetEnrollments(string indexNumber)
        {
            enrollments = new List<Enrollment>();

            using (var con = new SqlConnection(sqlConnection))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * from enrollment where IdEnrollment = (SELECT IdEnrollment from Student where student.IndexNumber = @indexNumber)";
                com.Parameters.AddWithValue("indexNumber", indexNumber);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var enrollment = new Enrollment();
                    enrollment.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    enrollment.Semester = int.Parse(dr["Semester"].ToString());
                    enrollment.IdStudy = int.Parse(dr["IdStudy"].ToString());
                    enrollment.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                    enrollments.Add(enrollment);

                }
            }
            return enrollments;
        }

        /*public ICollection<Student> GetStudents()
        {
            //połączenie z bazą sql con
            return null;
        }
        */

        IEnumerable<Student> IStudentsDel.GetStudents()
        {
            students = new List<Student>();

            using (var con = new SqlConnection(sqlConnection))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student";
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    students.Add(st);

                }
            }
            return students;
        }   
    }
}
