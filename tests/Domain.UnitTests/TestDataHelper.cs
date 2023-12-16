using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.UnitTests;

public static class TestDataHelper
{
    public static Cart GetTestCartData()
    {
        var list = new Cart
        {
            Id = 1,
            AppliedPromotionId = 1232,
            TotalDiscount = 2000,
            Items = new List<CartItem>
            {
                new()
                {
                    ItemId = 1001,
                    CategoryId = 3004,
                    SellerId = 1,
                    Quantity = 1,
                    Price = 1000,
                    CartId = 1,
                    VasItems = new List<VasItem>
                    {
                        new()
                        {
                            ItemId = 1001,
                            VasCategoryId = 3242,
                            VasSellerId = 5003,
                            Quantity = 1,
                            Price = 100,
                            VasItemId = 7001
                        },
                        new()
                        {
                            ItemId = 1001,
                            VasCategoryId = 3242,
                            VasSellerId = 5003,
                            Quantity = 1,
                            Price = 100,
                            VasItemId = 7002
                        }
                    }
                },
                new()
                {
                    ItemId = 1002,
                    CategoryId = 3003,
                    SellerId = 2,
                    Quantity = 2,
                    Price = 2000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1003,
                    CategoryId = 3003,
                    SellerId = 3,
                    Quantity = 3,
                    Price = 3000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1004,
                    CategoryId = 3003,
                    SellerId = 4,
                    Quantity = 4,
                    Price = 4000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1005,
                    CategoryId = 5,
                    SellerId = 5,
                    Quantity = 5,
                    Price = 5000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1006,
                    CategoryId = 6,
                    SellerId = 6,
                    Quantity = 2,
                    Price = 6000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1007,
                    CategoryId = 7,
                    SellerId = 7,
                    Quantity = 1,
                    Price = 7000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 5001,
                    CategoryId = 7889,
                    SellerId = 51,
                    Quantity = 2,
                    Price = 5001,
                    CartId = 1
                },
                new()
                {
                    ItemId = 5002,
                    CategoryId = 7889,
                    SellerId = 52,
                    Quantity = 5,
                    Price = 5002,
                    CartId = 1
                },
                new()
                {
                    ItemId = 5003,
                    CategoryId = 7889,
                    SellerId = 53,
                    Quantity = 4,
                    Price = 5003,
                    CartId = 1
                }
            }
        };

        return list;
    }

    public static Cart GetTestCartData_SameSeller()
    {
        var list = new Cart
        {
            Id = 1,
            AppliedPromotionId = 1232,
            TotalDiscount = 2000,
            Items = new List<CartItem>
            {
                new()
                {
                    ItemId = 1001,
                    CategoryId = 3004,
                    SellerId = 5003,
                    Quantity = 1,
                    Price = 1000,
                    CartId = 1,
                    VasItems = new List<VasItem>
                    {
                        new()
                        {
                            ItemId = 1001,
                            VasCategoryId = 3242,
                            VasSellerId = 5003,
                            Quantity = 1,
                            Price = 100,
                            VasItemId = 7001
                        },
                        new()
                        {
                            ItemId = 1001,
                            VasCategoryId = 3242,
                            VasSellerId = 5003,
                            Quantity = 1,
                            Price = 100,
                            VasItemId = 7002
                        }
                    }
                },
                new()
                {
                    ItemId = 1002,
                    CategoryId = 3003,
                    SellerId = 5003,
                    Quantity = 2,
                    Price = 2000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1003,
                    CategoryId = 3003,
                    SellerId = 5003,
                    Quantity = 3,
                    Price = 3000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1004,
                    CategoryId = 3003,
                    SellerId = 5003,
                    Quantity = 4,
                    Price = 4000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1005,
                    CategoryId = 5,
                    SellerId = 5003,
                    Quantity = 5,
                    Price = 5000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1006,
                    CategoryId = 6,
                    SellerId = 5003,
                    Quantity = 2,
                    Price = 6000,
                    CartId = 1
                },
                new()
                {
                    ItemId = 1007,
                    CategoryId = 7,
                    SellerId = 5003,
                    Quantity = 1,
                    Price = 7000,
                }
            }
        };
        return list;
    }
}