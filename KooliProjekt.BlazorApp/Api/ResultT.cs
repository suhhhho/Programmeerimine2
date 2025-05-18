namespace KooliProjekt.BlazorApp.Api
{
    public class Result<T> : Result
    {
        public T? Value { get; private set; }

        internal Result(T? value, bool success, Exception? error = null, string? errorMessage = null)
            : base(success, error, errorMessage)
        {
            Value = value;
        }
    }
}