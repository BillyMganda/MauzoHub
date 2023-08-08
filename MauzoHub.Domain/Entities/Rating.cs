namespace MauzoHub.Domain.Entities
{
    public class Rating
    {
        public int Value { get; }

        public Rating(int value)
        {
            if (value < 0 || value > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.", nameof(value));
            }

            Value = value;
        }
    }
}
