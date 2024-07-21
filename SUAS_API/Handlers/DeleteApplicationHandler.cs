using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class DeleteApplicationHandler : IRequestHandler<DeleteApplicationRequest, DeleteApplicationResponse>
    {
        private readonly AppDBContext _dbContext;

        public DeleteApplicationHandler(AppDBContext appDB)
        {
            _dbContext = appDB;
        }
        public async Task<DeleteApplicationResponse> Handle(DeleteApplicationRequest request, CancellationToken cancellationToken)
        {
            DeleteApplicationResponse response = new DeleteApplicationResponse();
            try
            {                
                var application = await _dbContext.Application.FindAsync(request.ApplicationID);
                if (application == null)
                {
                    response.Success = false;
                    response.Message = Constants.RecordNotFound;
                    return response;
                }
                _dbContext.Application.Remove(application);
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
