namespace ShoppingCart.Application.Carts.Commands.AddItem;

public class AddItemValidator : AbstractValidator<AddItemCommand>
{
    public AddItemValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId cannot be empty");
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId cannot be empty")
            .NotEqual(3242)
            .WithMessage("CategoryId cannot be 3242");
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity cannot be empty")
            .LessThanOrEqualTo(10)
            .WithMessage("Max item count is 10");
        RuleFor(x => x.Quantity)
            .LessThanOrEqualTo(5)
            .WithMessage("Max digital item count is 5")
            .When(x => x.CategoryId == 7889);
        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty");
        RuleFor(x => x.SellerId)
            .NotEmpty()
            .WithMessage("SellerId cannot be empty")
            .NotEqual(5003)
            .WithMessage("SellerId cannot be 5003");
    }
}