using SUAS_API.Models;

namespace SUAS_API.Responses
{
    public class UpdateCourseResponse
    {
        public Course? UpdatedCourseInfo { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int ResponseCode { get; set; }
    }
}
