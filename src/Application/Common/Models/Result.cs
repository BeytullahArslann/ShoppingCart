namespace ShoppingCart.Application.Common.Models;

public class ResultDto
{
    internal ResultDto(bool succeeded, string message)
    {
        Result = succeeded;
        Message = message;
    }

    public bool Result { get; init; }

    public string Message { get; init; }

    public static ResultDto Success()
    {
        return new ResultDto(true, "Success");
    }

    public static ResultDto Failure(IEnumerable<string> errors)
    {
        return new ResultDto(false, "Failure");
    }
}