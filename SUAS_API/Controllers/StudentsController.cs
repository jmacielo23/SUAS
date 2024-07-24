using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class StudentsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMediator _mediator;

        public StudentsController(AppDBContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult> GetStudents()
        {
            var query = new GetAllStudentsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudent(int id)
        {
            var query = new GetStudentQuery(id);
            var student = await _mediator.Send(query);
            return student == null ? NotFound() : Ok(student);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if(id!=student.ID)
            {
                return BadRequest();
            }
            var query = new UpdateStudentRequest(student);
            var response = await _mediator.Send(query);

            switch (response.ResponseCode)
            {
                case 404:
                    return NotFound(response.Message);
                case 200:
                    return Ok(response);
                default:
                    return BadRequest(response.Message);
            }
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new PostStudentRequest(student);
            var postStudentResponse = await _mediator.Send(command);

            return postStudentResponse.Success? Ok(postStudentResponse.StudentInfo): BadRequest(postStudentResponse.Message);           
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var command = new DeleteStudentRequest(id);
            var result = await _mediator.Send(command);

            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
