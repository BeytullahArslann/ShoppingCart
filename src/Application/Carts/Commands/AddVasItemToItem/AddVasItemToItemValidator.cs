namespace ShoppingCart.Application.Carts.Commands.AddVasItemToItem;

public class AddVasItemToItemValidator : AbstractValidator<AddVasItemToItemCommand>
{
    public AddVasItemToItemValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId cannot be empty");
        RuleFor(x => x.VasItemId)
            .NotEmpty()
            .WithMessage("VasItemId cannot be empty");
        RuleFor(x => x.VasCategoryId)
            .NotEmpty()
            .WithMessage("VasCategoryId cannot be empty")
            .Equal(3242)
            .WithMessage("VasCategoryId only can be 3242");
        RuleFor(x => x.VasSellerId)
            .NotEmpty()
            .WithMessage("VasSellerId cannot be empty")
            .Equal(5003)
            .WithMessage("VasSellerId only can be 5003");
        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty");
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity cannot be empty")
            .LessThanOrEqualTo(3)
            .WithMessage("Max vas item count is 3");
    }
}