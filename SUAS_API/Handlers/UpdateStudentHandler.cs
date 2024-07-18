using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentRequest, UpdateStudentResponse>
    {
        private readonly StudentsDBContext _dbContext;

        public UpdateStudentHandler(StudentsDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<UpdateStudentResponse> Handle(UpdateStudentRequest request, CancellationToken cancellationToken)
        {
            UpdateStudentResponse response = new UpdateStudentResponse();

            var existingStudent = await _dbContext.Students.FindAsync(request.StudentData.ID);

            if (existingStudent == null)
            {
                response.Success = false;
                response.Message = "Student not found.";
                response.ResponseCode = 404;
                return response;
            }

            _dbContext.Entry(request.StudentData).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Saved Successfully";
                response.UpdatedStudentInfo = await _dbContext.Students.FindAsync(request.StudentData.ID);
                response.ResponseCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                var errorReferenceNumber = Utility.LogTheError(ex);                
                response.Success = false;
                response.Message = $"Unable to save the changes. Error Reference: {errorReferenceNumber}";
                response.ResponseCode = 500;
                return response;
            }
        }
    }
}
