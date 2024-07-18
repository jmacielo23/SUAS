namespace SUAS_API.Helpers
{
    public static class Utility
    {
        public static string LogTheError(Exception exception) 
        {
            Guid errorReferenceNumber = Guid.NewGuid();
            //write the the exception in the log, use the error reference number as the identifier for faster troubleshooting
            //simulation of log writing here: Log.WriteError($"{errorReferenceNumber} | exception}
            return errorReferenceNumber.ToString();
        }
    }
}
