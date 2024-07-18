using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Commands
{
    public class UpdateStudentRequest:IRequest<UpdateStudentResponse>
    {
        public Student StudentData { get; }
        public UpdateStudentRequest(Student student)
        {
            StudentData = student;
        }
    }
}
