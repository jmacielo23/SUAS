using SUAS_API.Models;
using System.Reflection.Metadata.Ecma335;

namespace SUAS_API.Responses
{
    public class AddApplicationResponse
    {
        public Application? ApplicationInfo { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
