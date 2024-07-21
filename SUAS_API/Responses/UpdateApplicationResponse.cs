using SUAS_API.Models;

namespace SUAS_API.Responses
{
    public class UpdateApplicationResponse
    {
        public Application? ApplicationInfo { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int ResponseCode { get; set; }
    }
}
