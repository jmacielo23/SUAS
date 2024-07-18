using MediatR;
using SUAS_API.Models;

namespace SUAS_API.Queries
{
    public class GetStudentQuery :IRequest<Student>
    {
        public int StudentID { get; }

        public GetStudentQuery(int studentID)
        {
            StudentID = studentID;
        }

    }
}
