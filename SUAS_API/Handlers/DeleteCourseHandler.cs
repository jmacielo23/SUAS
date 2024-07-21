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
                response.Message = "Successfully deleted the Course";
                return response;
            }
            catch (Exception ex)
            {
                var errorRefNumber = Utility.LogTheError(ex);
                response.Success = false;
                response.Message = $"An Error Occured. Error Reference Number: {errorRefNumber}";
                return response;
            }
        }
    }
}
