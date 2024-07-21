using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Commands
{
    public class UpdateApplicationRequest:IRequest<UpdateApplicationResponse>
    {
        public Application ApplicationData { get; }
        public UpdateApplicationRequest(Application applicationData)
        {
            ApplicationData = applicationData;
        }
    }
}
