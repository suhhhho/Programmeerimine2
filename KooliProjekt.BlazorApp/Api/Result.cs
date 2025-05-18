using System;
using System.Collections.Generic;

namespace KooliProjekt.BlazorApp.Api
{
    public class Result
    {
        public bool Success { get; private set; }
        public Exception? Error { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Dictionary<string, List<string>> ValidationErrors { get; private set; } = new();

        protected Result(bool success, Exception? error = null, string? errorMessage = null)
        {
            Success = success;
            Error = error;
            ErrorMessage = errorMessage ?? error?.Message;
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
}