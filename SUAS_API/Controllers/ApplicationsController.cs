using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using MediatR;
using SUAS_API.Queries;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SUAS_API.Commands;
using System.Net.Http.Headers;
using SUAS_API.Helpers;

namespace SUAS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMediator _mediator;

        public ApplicationsController(AppDBContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplication()
        {
            var query = new GetAllApplicationsQuery();
            return Ok(await _mediator.Send(query));
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var query = new GetApplicationQuery(id);
            var response = await _mediator.Send(query);
            return response==null?NotFound("Application Not Found"):Ok(response);
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, Application application)
        {
            if (id != application.ID)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostApplication(Application application)
        {
            var query = new AddApplicationRequest(application);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.ApplicationInfo) : BadRequest(response.Message);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Application.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Application.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.ID == id);
        }
    }
}
