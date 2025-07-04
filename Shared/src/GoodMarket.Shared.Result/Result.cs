namespace GoodMarket.Shared.Result;

public class Result
{
    public ResultCode Code { get; protected set; }
    public IEnumerable<Error> Errors { get; set; } = new List<Error>();


    public bool Success => Code == ResultCode.Ok;
    public bool EntityNotFound => Code == ResultCode.EntityNotFound;
    public bool InternalError => Code == ResultCode.InternalError;
    public bool BadRequest => Code == ResultCode.BadRequest;
    public bool Forbidden => Code == ResultCode.Forbidden;


    public static Result SuccessResult() => new() { Code = ResultCode.Ok };
    public static Result EntityNotFoundResult() => new() { Code = ResultCode.EntityNotFound };
    public static Result InternalErrorResult() => new() { Code = ResultCode.InternalError };
    public static Result BadRequestResult() => new() { Code = ResultCode.BadRequest };
    public static Result Unauthorized() => new() { Code = ResultCode.Unauthorized };

    public static Result BadRequestResult(List<Error> errors) => new()
    {
        Code = ResultCode.BadRequest,
        Errors = errors
    };

    public static Result ForbiddenResult() => new() { Code = ResultCode.Forbidden };
}

public class Result<T> : Result
{
    public T? Data { get; set; }


    public Result()
    {
    }

    public Result(Result result)
    {
        Code = result.Code;
        Errors = result.Errors;
    }


    public static implicit operator Result<T>(T value) => new Result<T>
    {
        Code = ResultCode.Ok,
        Data = value
    };

    public static implicit operator Result<T>(Error error) => new Result<T>
    {
        Code = ResultCode.InternalError,
        Errors = [error]
    };
}

public class Error
{
    public string Code { get; private set; }
    public string Message { get; private set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}