using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;
namespace SUAS_API.Commands
{
    public class UpdateCourseRequest:IRequest<UpdateCourseResponse>
    {
        public Course CourseInfo { get; }
        public UpdateCourseRequest(Course courseInfo)
        {
            CourseInfo = courseInfo;
        }
    }
}
