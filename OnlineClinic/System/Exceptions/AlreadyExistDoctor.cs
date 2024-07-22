namespace OnlineClinic.System.Exceptions
{
    public class AlreadyExistDoctor : Exception
    {
        public AlreadyExistDoctor(string? message):base(message) { }
    }
}
