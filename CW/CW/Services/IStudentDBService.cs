using CW.DTOs.Request;
using CW.DTOs.Response;
using CW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Services
{
    public interface IStudentDBService
    {
        ResponseState EnrollStudent(EnrollStudentRequest request);
        //void EnrollStudent(EnrollStudentRequest request);
        //void EnrollStudent(Student newStudent);

        //void PromoteStudents(int semester, string studies);
        
        ResponseState PromoteStudents(Promote_StudentRequest request);
        Student GetStudent(string index);
        
    }
}
