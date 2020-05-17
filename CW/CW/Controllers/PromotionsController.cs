using CW.DTOs.Request;
using CW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Controllers
{
    [Route("api/enrollments/promotions")]
    [ApiController]
public class PromotionsController : ControllerBase
    {
        private IStudentDBService _service;
        public PromotionsController(IStudentDBService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(Promote_StudentRequest promotion)
        {
            var response = _service.PromoteStudents(promotion);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
    }

   
}
