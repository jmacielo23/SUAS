using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Responses;
using System.Threading.Tasks.Dataflow;

namespace SUAS_API.Handlers
{
    public class AddApplicationHandler:IRequestHandler<AddApplicationRequest,AddApplicationResponse>
    {
        private readonly AppDBContext _dbContext;

        public AddApplicationHandler(AppDBContext appDB)
        {
            _dbContext = appDB;
        }

        public async Task<AddApplicationResponse> Handle(AddApplicationRequest request, CancellationToken cancellationToken)
        {
            AddApplicationResponse response = new AddApplicationResponse();
            try
            {
                //check if the passed ID is existing
                var existingApplication = await _dbContext.Application.FindAsync(request.ApplicationData.ID);
                if (existingApplication != null)
                {
                    response.Success = false;
                    response.Message = Constants.RecordExisting;
                    response.ApplicationInfo = null;
                    return response;
                }
                //check if the passed student ID is existing
                var student = await _dbContext.Students.FindAsync(request.ApplicationData.StudentID);
                if (student == null) //return bad request
                {
                    response.Message = Constants.StudentNotFound;
                    response.Success = false;
                    response.ApplicationInfo = null;
                    return response;
                }
                //check if the passed Course ID is existing
                var course = await _dbContext.Courses.FindAsync(request.ApplicationData.CourseID);
                if(course==null) //return bad request
                {
                    response.Message = Constants.CourseNotFound;
                    response.Success = false;
                    response.ApplicationInfo = null;
                    return response;
                }
                _dbContext.Application.Add(request.ApplicationData);
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.ApplicationInfo=request.ApplicationData;
                response.Message = Constants.RecordSaved;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message=Utility.GenericErrorMessage(Utility.LogTheError(ex));
                response.ApplicationInfo = null;
                return response;
            }
        }
    }
}
