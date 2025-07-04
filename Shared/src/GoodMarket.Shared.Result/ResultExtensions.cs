namespace GoodMarket.Shared.Result;

 public static class ResultExtensions
    {
        #region Public Methods
        public static Result WithError(this Result result, string message, string code = DefaultErrorCodes.OperationFailure)
        {
            return result.AddError(code, message);
        }

        public static Result WithError(this Result result, Error error)
        {
            return result.AddError(error);
        }

        public static Result WithErrors(this Result result, IEnumerable<string> messages, string code = DefaultErrorCodes.OperationFailure)
        {
            return result.AddErrors(code, messages);
        }

        public static Result WithErrors(this Result result, IEnumerable<Error> errors)
        {
            return result.AddErrors(errors);
        }

        public static Result<T> WithError<T>(this Result result, string message, string code = DefaultErrorCodes.OperationFailure)
        {
            return new Result<T>(result).AddError(code, message);
        }

        public static Result<T> WithError<T>(this Result<T> result, Error error)
        {
            return new Result<T>(result).AddError(error);
        }

        public static Result<T> WithErrors<T>(this Result result, IEnumerable<string> messages, string code = DefaultErrorCodes.OperationFailure)
        {
            return new Result<T>(result).AddErrors(code, messages);
        }

        public static Result<T> WithErrors<T>(this Result result, IEnumerable<Error> errors)
        {
            return new Result<T>(result).AddErrors(errors);
        }


        public static Result<T> WithData<T>(this Result result, T data)
        {
            return new Result<T>(result).SetData(data);
        }

        public static Result<T> WithEmptyData<T>(this Result result)
        {
            return new Result<T>(result);
        }

        public static Result AddError(this Result result, string code, string message)
        {
            var error = new Error(code, message);
            result.Errors = new List<Error> { error };
            return result;
        }
        #endregion

        #region Private Methods
        private static Result AddError(this Result result, Error error)
        {
            result.Errors = new List<Error> { error };
            return result;
        }

        private static Result AddErrors(this Result result, string code, IEnumerable<string> messages)
        {
            var errors = new List<Error>();
            foreach (var message in messages)
                errors.Add(new Error(code, message));

            result.Errors = errors;
            return result;
        }

        private static Result AddErrors(this Result result, IEnumerable<Error> errors)
        {
            result.Errors = errors;
            return result;
        }

        private static Result<T> AddError<T>(this Result<T> result, string code, string message)
        {
            var error = new Error(code, message);
            result.Errors = new List<Error> { error };
            return result;
        }

        private static Result<T> AddError<T>(this Result<T> result, Error error)
        {
            result.Errors = new List<Error> { error };
            return result;
        }

        private static Result<T> AddErrors<T>(this Result<T> result, string code, IEnumerable<string> messages)
        {
            var errors = new List<Error>();
            foreach (var message in messages)
                errors.Add(new Error(code, message));

            result.Errors = errors;
            return result;
        }

        private static Result<T> AddErrors<T>(this Result<T> result, IEnumerable<Error> errors)
        {
            result.Errors = errors;
            return result;
        }

        private static Result<T> SetData<T>(this Result<T> result, T data)
        {
            result.Data = data;
            return result;
        }
        #endregion
    }