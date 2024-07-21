using SUAS_API.Models;

namespace SUAS_API.Responses
{
    public class AddCourseResponse
    {
        public Course? CourseInfo { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
