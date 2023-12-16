namespace ShoppingCart.Application.Carts.Commands.RemoveItem;

public class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
{
    public RemoveItemValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId cannot be empty");
    }
}