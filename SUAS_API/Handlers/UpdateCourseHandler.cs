using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class UpdateCourseHandler:IRequestHandler<UpdateCourseRequest,UpdateCourseResponse>
    {
        private readonly AppDBContext _dbContext;

        public UpdateCourseHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }

        public async Task<UpdateCourseResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
        {
            UpdateCourseResponse response = new UpdateCourseResponse();
            try
            {
                var existingCourse = await _dbContext.Courses.FindAsync(request.CourseInfo.ID);

                if(existingCourse == null) 
                {
                    response.Success = false;
                    response.Message = "Course not found.";
                    response.ResponseCode = 404;
                    return response;
                }

                existingCourse.Title = request.CourseInfo.Title;
                existingCourse.Credits = request.CourseInfo.Credits;
                existingCourse.Code=request.CourseInfo.Code;

                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Message = "Saved Succefully";
                response.UpdatedCourseInfo = await _dbContext.Courses.FindAsync(request.CourseInfo.ID);
                response.ResponseCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.UpdatedCourseInfo = null;
                response.ResponseCode = 500;
                response.Message=Utility.GenericErrorMessage(Utility.LogTheError(ex));
                return response;
            }
        }
    }
}
