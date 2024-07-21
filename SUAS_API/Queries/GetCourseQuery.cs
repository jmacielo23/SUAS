using MediatR;
using SUAS_API.Models;

namespace SUAS_API.Queries
{
    public class GetCourseQuery : IRequest<Course>
    {
        public int CourseID { get; }
        public GetCourseQuery(int courseID)
        {
            CourseID = courseID;
        }
    }
}
