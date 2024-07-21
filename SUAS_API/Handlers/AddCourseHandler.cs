using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Models;
using SUAS_API.Responses;
using System.Drawing.Text;

namespace SUAS_API.Handlers
{
    public class AddCourseHandler : IRequestHandler<AddCourseRequest, AddCourseResponse>
    {
        private readonly AppDBContext _dbContext;

        public AddCourseHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<AddCourseResponse> Handle(AddCourseRequest request, CancellationToken cancellationToken)
        {
            AddCourseResponse response = new AddCourseResponse();
            try
            {
                var existingCourse = await _dbContext.Courses.FindAsync(request.CourseData.ID);
                if (existingCourse == null)
                {
                    _dbContext.Courses.Add(request.CourseData);
                    await _dbContext.SaveChangesAsync();
                    response.CourseInfo = request.CourseData;
                    response.Success = true;
                    response.Message = "Course Saved.";
                    return response;
                }
                response.Message = "Course is existing.";
                response.Success = false;
                response.CourseInfo = null;
                return response;
            }
            catch (Exception ex)
            {
                string errorReferenceNumber = Utility.LogTheError(ex);
                response.Success = false;
                response.Message = $"Unable to save the course. Error Reference Number: {errorReferenceNumber}";
                response.CourseInfo = null;
                return response;
            }


        }
    }
}
