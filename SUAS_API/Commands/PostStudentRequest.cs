using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Commands
{
    public class PostStudentRequest:IRequest<PostStudentResponse>
    {
        public Student Student { get; }

        public PostStudentRequest(Student student)
        {
            Student = student;
        }
    }
}
