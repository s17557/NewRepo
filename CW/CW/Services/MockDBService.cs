using CW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    public class MockDBService : IDBService
    {   
        //tworzenie listy
        private static IEnumerable<Student> _students = new List<Student>
        {
            new Student {IdStudent=1, FirstName= "Jan", LastName="Kowalski", IndexNumber="s1234"},
            new Student {IdStudent=2, FirstName= "Anna", LastName="Malewski", IndexNumber="s2345"},
            new Student {IdStudent=3, FirstName= "Krzysztof", LastName="Malinowski", IndexNumber="s3456"},
        };

        public IEnumerable<Enrollment> GetEnrollments(string indexNumber)
        {
            throw new NotImplementedException();
        }

        //zwracanie listy
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
