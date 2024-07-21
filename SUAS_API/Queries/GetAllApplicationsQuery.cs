using MediatR;
using SUAS_API.Models;
namespace SUAS_API.Queries
{
    public class GetAllApplicationsQuery:IRequest<IEnumerable<Application>>
    {
    }
}
