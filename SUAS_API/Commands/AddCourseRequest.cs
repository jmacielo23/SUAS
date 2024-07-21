using MediatR;
using SUAS_API.Models;
using SUAS_API.Responses;
namespace SUAS_API.Commands
{
    public class AddCourseRequest:IRequest<AddCourseResponse>
    {
        public Course CourseData { get; }
        public AddCourseRequest(Course course)
        {
            CourseData = course;
        }
    }
}
