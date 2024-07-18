using MediatR;
using SUAS_API.Responses;

namespace SUAS_API.Commands
{
    public class DeleteStudentRequest :IRequest<DeleteStudentResponse>
    {
        public int StudentID { get; }
        public DeleteStudentRequest(int studentID)
        {
            StudentID = studentID;
        }
    }
}
