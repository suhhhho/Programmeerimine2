using System;

namespace KooliProjekt.WpfApp.Api
{
    public class Result
    {
        public bool Success { get; protected set; }
        public string Error { get; protected set; }

        protected Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error);
        }

        public static Result Fail(Exception ex)
        {
            return new Result(false, ex.Message);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(default, false, error);
        }

        public static Result<T> Fail<T>(Exception ex)
        {
            return new Result<T>(default, false, ex.Message);
        }
    }
}