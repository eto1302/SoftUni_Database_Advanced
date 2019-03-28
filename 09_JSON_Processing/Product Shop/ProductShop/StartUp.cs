using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Runtime.Serialization.Json;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var Users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(Users);
            context.SaveChanges();
            return $"Successfully imported {Users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var Products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.Products.AddRange(Products);
            context.SaveChanges();
            return $"Successfully imported {Products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            Category[] Categories = JsonConvert.DeserializeObject<Category[]>(inputJson).Where(c => c.Name != null).ToArray();
            context.Categories.AddRange(Categories);
            context.SaveChanges();
            return $"Successfully imported {Categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(
                p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName +" " + p.Seller.LastName
                })
                .ToArray();

            return JsonConvert.SerializeObject(products);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(x => x.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                }).ToArray();

            return JsonConvert.SerializeObject(users);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("F2")

                })
                .ToArray();

            return JsonConvert.SerializeObject(categories);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = new
            {
                usersCount = context.Users.Count(),
                users = context.Users
                    .OrderByDescending(x => x.ProductsSold.Count)
                    .ThenBy(l => l.LastName)
                    .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(s => s.Buyer != null))
                    .Select(x => new
                    {
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        age = x.Age,
                        soldProducts = new
                        {
                            count = x.ProductsSold.Count,
                            products = x.ProductsSold.Select(s => new
                            {
                                name = s.Name,
                                price = s.Price
                            })
                        }
                    })
            };

           

            return JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}