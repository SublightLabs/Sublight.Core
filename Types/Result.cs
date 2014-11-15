using System;

namespace Sublight.Core.Types
{
    public class Result<T>
    {
        private Result()
        {
        }

        public static Result<T> CreateSuccess(T value)
        {
            return new Result<T>() {Status = ResultStatus.Success, Value = value};
        }

        public static Result<T> CreateError(string message)
        {
            return new Result<T>() { Status = ResultStatus.Error, ErrorMessage = message };
        }

        public static Result<T> CreateException(Exception exception)
        {
            return new Result<T>() {Status = ResultStatus.Exception, Exception = exception};
        }

        public Exception Exception
        {
            get; private set;
        }

        public string ErrorMessage
        {
            get; private set;
        }

        public T Value
        {
            get; private set;
        }

        public ResultStatus Status
        {
            get;
            private set;
        }

        public enum ResultStatus
        {
            Success,
            Error,
            Exception
        }
    }
}
