using GoodMarket.Shared.Result;

namespace GoodMarket.Identity.Api.Shared.Extensions;

public static class CustomResultExtensions
{
    public static IResult CustomResult(this Result result)
    {
        if (!result.Success)
            return result.Error();
        
        return Results.NoContent();
    }
    
    public static IResult CustomResult<T>(this Result<T> result)
    {
        if (!result.Success)
            return result.Error();
        
        return Results.Ok(result.Data);
    }
    
    public static IResult Error(this Result result)
    {
        return result.Code switch
        {
            ResultCode.InternalError => Results.InternalServerError(),
            ResultCode.EntityNotFound => Results.NotFound(),
            ResultCode.Unauthorized => Results.Unauthorized(),
            ResultCode.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(result.Errors)
        };
    }
}