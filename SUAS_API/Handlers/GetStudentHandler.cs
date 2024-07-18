using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetStudentHandler : IRequestHandler<GetStudentQuery, Student>
    {
        private readonly AppDBContext _dbContext;

        public GetStudentHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }

        public async Task<Student> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await _dbContext.Students.FindAsync(request.StudentID);
            return student == null ? null : student;
        }
    }
}
