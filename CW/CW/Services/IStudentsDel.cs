using CW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    interface IStudentsDel
    {
       //public ICollection<Student> GetStudents();
       public IEnumerable<Student> GetStudents();
       public IEnumerable<Enrollment> GetEnrollments(string indexNumber);

    }
}

