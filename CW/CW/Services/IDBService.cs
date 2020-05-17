using CW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    public interface IDBService
    {
        public IEnumerable<Student> GetStudents();
        public IEnumerable<Enrollment> GetEnrollments(string indexNumber);
    }
}
