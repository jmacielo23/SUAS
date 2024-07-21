using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetAllApplicationsHandler:IRequestHandler<GetAllApplicationsQuery,IEnumerable<Application>>
    {
        private readonly AppDBContext _dbContext;

        public GetAllApplicationsHandler(AppDBContext appDB)
        {
            _dbContext = appDB;
        }

        public async Task<IEnumerable<Application>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Application.ToListAsync();
        }
    }
}
