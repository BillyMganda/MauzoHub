namespace MauzoHub.Application.CustomExceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {

        }
    }
}
