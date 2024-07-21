using MediatR;
using SUAS_API.Responses;
namespace SUAS_API.Commands
{
    public class DeleteApplicationRequest:IRequest<DeleteApplicationResponse>
    {
        public int ApplicationID { get; }
        public DeleteApplicationRequest(int applicationID)
        {
            ApplicationID=applicationID;
        }
    }
}
