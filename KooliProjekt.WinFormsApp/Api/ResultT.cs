namespace KooliProjekt.WinFormsApp.Api
{
    public class Result<T> : Result
    {
        public T Value { get; }

        internal Result(T value, bool success, string error) : base(success, error)
        {
            Value = value;
        }
    }
}