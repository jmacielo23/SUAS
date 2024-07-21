using MediatR;
using SUAS_API.Models;
namespace SUAS_API.Queries
{
    public class GetApplicationQuery:IRequest<Application>
    {
        public int ApplicationID { get; }
        public GetApplicationQuery(int applicationID)
        {
            ApplicationID = applicationID;
        }
    }
}
