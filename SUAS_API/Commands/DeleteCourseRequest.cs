using MediatR;
using SUAS_API.Responses;
namespace SUAS_API.Commands
{
    public class DeleteCourseRequest:IRequest<DeleteCourseResponse>
    {
        public int CourseId { get; }
        public DeleteCourseRequest(int courseID)
        {
            CourseId = courseID;
        }
    }
}
