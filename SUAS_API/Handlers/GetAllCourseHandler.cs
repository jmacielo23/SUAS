using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetAllCourseHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<Course>>
    {
        private readonly AppDBContext _dbContext;

        public GetAllCourseHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<IEnumerable<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Courses.ToListAsync();
        }
    }
}
