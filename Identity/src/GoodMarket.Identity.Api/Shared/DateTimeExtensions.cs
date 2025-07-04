namespace GoodMarket.Identity.Api.Shared;

public static class DateTimeHelpers
{
    public static DateTime CreateUtcDate(int year, int month, int day) =>
        new(year, month, day, 0, 0, 0, DateTimeKind.Utc);
    
}