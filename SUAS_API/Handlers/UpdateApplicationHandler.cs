using MediatR;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class UpdateApplicationHandler:IRequestHandler<UpdateApplicationRequest,UpdateApplicationResponse>
    {
        private readonly AppDBContext _dbContext;

        public UpdateApplicationHandler(AppDBContext appDB)
        {
            _dbContext = appDB;
        }

        public async Task<UpdateApplicationResponse> Handle(UpdateApplicationRequest request, CancellationToken cancellationToken)
        {
            UpdateApplicationResponse response = new UpdateApplicationResponse();
            try
            {
                var existingApplication = await _dbContext.Application.FindAsync(request.ApplicationData.ID);

                if (existingApplication == null)
                {
                    response.Success = false;
                    response.Message = Constants.RecordNotFound;
                    response.ResponseCode = 404;
                    response.ApplicationInfo = null;
                    return response;
                }

                var student = await _dbContext.Students.FindAsync(request.ApplicationData.StudentID);
                var course = await _dbContext.Courses.FindAsync(request.ApplicationData.CourseID);

                if (student == null)
                {
                    response.Success = false;
                    response.Message = Constants.StudentNotFound;
                    response.ResponseCode = 400;
                    response.ApplicationInfo = null;
                    return response;
                }
                else if (course == null)
                {
                    response.Success = false;
                    response.Message = Constants.CourseNotFound;
                    response.ResponseCode = 400;
                    response.ApplicationInfo = null;
                    return response;
                }
                else
                {
                    existingApplication.StudentID = request.ApplicationData.StudentID;
                    existingApplication.CourseID = request.ApplicationData.CourseID;
                    existingApplication.ApplicationDate = request.ApplicationData.ApplicationDate;

                    await _dbContext.SaveChangesAsync();

                    response.Success = true;
                    response.Message = Constants.RecordSaved;
                    response.ApplicationInfo = request.ApplicationData;
                    response.ResponseCode = 200;
                    return response;
                }
;            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = Utility.GenericErrorMessage(Utility.LogTheError(ex));
                response.ResponseCode = 500;
                return response;
            }
        }
    }
}
