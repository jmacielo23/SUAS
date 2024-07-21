namespace SUAS_API.Models
{
    public class Application
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int CourseID  { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
