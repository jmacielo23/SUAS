using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentRequest, DeleteStudentResponse>
    {
        private readonly StudentsDBContext _dbContext;

        public DeleteStudentHandler(StudentsDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<DeleteStudentResponse> Handle(DeleteStudentRequest request, CancellationToken cancellationToken)
        {
            DeleteStudentResponse response = new DeleteStudentResponse();
            //check if the student id is existing in the context
            var student = await _dbContext.Students.FindAsync(request.StudentID);
            if (student == null)
            {
                response.Success = false;
                response.Message = "Student is not existing";
                return response;
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            response.Success = true;
            response.Message = "Student Deleted Successfully";
            return response;

        }
    }
}
