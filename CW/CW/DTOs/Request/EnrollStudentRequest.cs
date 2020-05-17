using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW.DTOs.Request
{
    public class EnrollStudentRequest
    {
        public int IdEnrollment { get; set; }
        public int IdStudent { get; set; }
        [Required(ErrorMessage = "Musisz podać imie")] //walidacja - dana wymagana
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Musisz podać nazwisko")]
        public string LastName { get; set; }
        [RegularExpression("^s[0-9]+$")]
        [MaxLength(10)]
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Studies { get; set; }
        public int Semester { get; set; }

    }
}
