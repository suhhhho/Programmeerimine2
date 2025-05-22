// KooliProjekt.BlazorApp/Api/Result.cs
using System;
using System.Collections.Generic;

namespace KooliProjekt.BlazorApp.Api
{
    public class Result
    {
        public bool Success { get; protected set; }
        public string Error { get; protected set; }

        // Add this property to maintain compatibility with components
        public string ErrorMessage => Error;

        public Exception? Exception { get; protected set; }
        public Dictionary<string, List<string>> ValidationErrors { get; protected set; } = new();

        protected Result(bool success, Exception? exception = null, string? error = null)
        {
            Success = success;
            Exception = exception;
            Error = error ?? exception?.Message ?? string.Empty;
        }

        public static Result Ok()
        {
            return new Result(true);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true);
        }

        public static Result Fail(Exception ex)
        {
            return new Result(false, ex);
        }

        public static Result Fail(string errorMessage)
        {
            return new Result(false, null, errorMessage);
        }

        public static Result Fail(Dictionary<string, List<string>> validationErrors)
        {
            var result = new Result(false, null, "Validation failed");
            result.ValidationErrors = validationErrors;
            return result;
        }

        public static Result<T> Fail<T>(Exception ex)
        {
            return new Result<T>(default, false, ex);
        }

        public static Result<T> Fail<T>(string errorMessage)
        {
            return new Result<T>(default, false, null, errorMessage);
        }

        public static Result<T> Fail<T>(Dictionary<string, List<string>> validationErrors)
        {
            var result = new Result<T>(default, false, null, "Validation failed");
            result.ValidationErrors = validationErrors;
            return result;
        }

        public bool HasValidationErrors => ValidationErrors.Count > 0;
    }

    public class Result<T> : Result
    {
        public T? Value { get; protected set; }

        internal Result(T? value, bool success, Exception? exception = null, string? error = null)
            : base(success, exception, error)
        {
            Value = value;
        }
    }
}
