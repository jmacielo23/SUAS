using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Data;
using SUAS_API.Models;
using SUAS_API.Queries;

namespace SUAS_API.Handlers
{
    public class GetApplicationHandler:IRequestHandler<GetApplicationQuery,Application>
    {
        private readonly AppDBContext _dbContext;

        public GetApplicationHandler(AppDBContext appDB)
        {
            _dbContext = appDB;
        }

        public async Task<Application> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _dbContext.Application.FindAsync(request.ApplicationID);
            return application == null ? null : application;
        }
    }
}
