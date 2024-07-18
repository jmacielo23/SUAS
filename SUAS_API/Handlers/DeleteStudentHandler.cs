using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentRequest, DeleteStudentResponse>
    {
        private readonly AppDBContext _dbContext;

        public DeleteStudentHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<DeleteStudentResponse> Handle(DeleteStudentRequest request, CancellationToken cancellationToken)
        {
            DeleteStudentResponse response = new DeleteStudentResponse();
            //check if the student id is existing in the context
            try
            {
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
            catch (Exception ex)
            {
                var ErrorReferenceNumber = Utility.LogTheError(ex);
                response.Success = false;
                response.Message = $"Unable to delete the record. Error Reference Number: {ErrorReferenceNumber}";
                return response;
            }

        }
    }
}
