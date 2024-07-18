using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<Student>>
    {
        private readonly AppDBContext _dbContext;

        public GetAllStudentsHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }
        public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
           return await _dbContext.Students.ToListAsync();
        }
    }
}
