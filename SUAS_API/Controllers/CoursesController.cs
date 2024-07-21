using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Models;
using SUAS_API.Queries;
using SUAS_API.Responses;

namespace SUAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMediator _mediator;

        public CoursesController(AppDBContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var query = new GetAllCoursesQuery();
            var courses = await _mediator.Send(query);
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            var query = new GetCourseQuery(id);
            var course = await _mediator.Send(query);
            return course == null ? NotFound("Course Not Existing") : Ok(course);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCourse(Course course)
        {
            var query = new UpdateCourseRequest(course);
            var response = await _mediator.Send(query);
            switch (response.ResponseCode)
            {
                case 404:
                    return NotFound(response.Message);
                case 200:
                    return Ok(response.UpdatedCourseInfo);
                default:
                    return BadRequest(response.Message);
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            var query = new AddCourseRequest(course);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.CourseInfo) : BadRequest(response.Message);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var query = new DeleteCourseRequest(id);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Message) : response.Message != null && response.Message.Equals(Constants.RecordNotFound) ? NotFound(response.Message) : BadRequest(response.Message);
        }
    }
}
