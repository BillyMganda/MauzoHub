namespace MauzoHub.Application.CustomExceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(string message) : base(message)
        {

        }
    }
}
