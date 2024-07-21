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
            
            try
            {
                //check if the student is linked in an application
                var application = await _dbContext.Application.AnyAsync(a=>a.StudentID==request.StudentID);
                if (application)
                {
                    response.Success = false;
                    response.Message = Constants.RecordLinkedInApplication;
                    return response;
                }
                //check if the student id is existing in the context
                var student = await _dbContext.Students.FindAsync(request.StudentID);
                if (student == null)
                {
                    response.Success = false;
                    response.Message = Constants.RecordNotFound;
                    return response;
                }
                _dbContext.Students.Remove(student);
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = Constants.RecordDeleted;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = Utility.GenericErrorMessage(Utility.LogTheError(ex));
                return response;
            }

        }
    }
}
