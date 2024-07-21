using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;
namespace SUAS_API.Commands
{
    public class AddApplicationRequest:IRequest<AddApplicationResponse>
    {
        public Application ApplicationData { get; }
        public AddApplicationRequest(Application applicationData)
        {
            ApplicationData = applicationData;
        }
    }
}
