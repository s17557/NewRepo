using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.DTOs.Response
{
    public class EnrollStudentResponse
    {
        public string LastName { get; set; }
        public int Semestr { get; set; }
        public DateTime StartDate { get; set; }
    }
}
