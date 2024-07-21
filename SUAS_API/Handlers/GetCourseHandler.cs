using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetCourseHandler : IRequestHandler<GetCourseQuery, Course>
    {
        private readonly AppDBContext _dbContext;

        public GetCourseHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<Course> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _dbContext.Courses.FindAsync(request.CourseID);
            return course == null ? null : course;
        }
    }
}
