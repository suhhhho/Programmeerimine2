using System;

namespace KooliProjekt.WinFormsApp.Api
{
    public class Result
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        protected Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static Result Ok() => new Result(true, null);
        public static Result Fail(string error) => new Result(false, error);
        public static Result Fail(Exception ex) => new Result(false, ex.Message);
        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, null);
        public static Result<T> Fail<T>(string error) => new Result<T>(default, false, error);
        public static Result<T> Fail<T>(Exception ex) => new Result<T>(default, false, ex.Message);
    }
}