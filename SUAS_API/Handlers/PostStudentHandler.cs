using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class PostStudentHandler:IRequestHandler<PostStudentRequest,PostStudentResponse>
    {
        private readonly StudentsDBContext _dbContext;

        public PostStudentHandler(StudentsDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }

        public async Task<PostStudentResponse> Handle(PostStudentRequest request, CancellationToken cancellationToken)
        {
            PostStudentResponse response = new PostStudentResponse();
            response.StudentInfo = request.Student;
            response.Success = false;
            var existingStudentRecord = await _dbContext.Students.FindAsync(request.Student.ID);
            if (existingStudentRecord == null)
            {
                _dbContext.Students.Add(request.Student);
                await _dbContext.SaveChangesAsync();                
                response.Success = true;
            }
            return response;
        }
    }
}
