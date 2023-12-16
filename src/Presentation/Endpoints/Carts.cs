using ShoppingCart.Application.Carts.Commands.AddItem;
using ShoppingCart.Application.Carts.Commands.AddVasItemToItem;
using ShoppingCart.Application.Carts.Commands.DisplayCart;
using ShoppingCart.Application.Carts.Commands.RemoveItem;
using ShoppingCart.Application.Carts.Commands.ResetCart;
using ShoppingCart.Application.Common.Models;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Presentation.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Presentation.Endpoints;

public class Carts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(DisplayCart, "/DisplayCart")
            .MapPost(AddItem, "/AddItem")
            .MapPost(AddVasItemToItem, "/AddVasItemToItem")
            .MapDelete(RemoveItem, "/RemoveItem")
            .MapDelete(ResetCart, "/ResetCart");
    }

    public async Task<Cart> DisplayCart(ISender sender)
    {
        return await sender.Send(new DisplayCartCommand());
    }

    public async Task<ResultDto> AddItem(ISender sender, AddItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<ResultDto> AddVasItemToItem(ISender sender, AddVasItemToItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<ResultDto> RemoveItem(ISender sender, [FromQuery] int itemId)
    {
        return await sender.Send(new RemoveItemCommand(itemId));
    }

    public async Task<ResultDto> ResetCart(ISender sender)
    {
        return await sender.Send(new ResetCartCommand());
    }
}