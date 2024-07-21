using SUAS_API.Models;

namespace SUAS_API.Responses
{
    public class PostStudentResponse
    {
        public Student? StudentInfo { get; set; }
        public bool Success { get; set; }

        public string? Message { get; set; }
    }
}
