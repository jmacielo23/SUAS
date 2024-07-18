using MediatR;
using SUAS_API.Models;

namespace SUAS_API.Queries
{
    public class GetAllStudentsQuery : IRequest<IEnumerable<Student>>
    {

    }
}
