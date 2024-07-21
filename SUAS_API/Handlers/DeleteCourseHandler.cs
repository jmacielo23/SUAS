using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseRequest, DeleteCourseResponse>
    {
        private readonly AppDBContext _dbContext;

        public DeleteCourseHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<DeleteCourseResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
        {
            DeleteCourseResponse response = new DeleteCourseResponse();
            try
            {
                //check if the course to be deleted is linked to an application
                var application = await _dbContext.Application.AnyAsync(a=>a.CourseID == request.CourseId);

                if (application)
                {
                    response.Message = Constants.RecordLinkedInApplication;
                    response.Success = false;
                    return response;
                }

                var course = await _dbContext.Courses.FindAsync(request.CourseId);
                if (course == null)
                {
                    response.Message = Constants.RecordNotFound;
                    response.Success = false;
                    return response;
                }
                _dbContext.Courses.Remove(course);
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
