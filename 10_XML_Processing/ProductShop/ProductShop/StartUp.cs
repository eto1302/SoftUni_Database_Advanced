using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(User[]), new XmlRootAttribute("Users"));

            var Users =
                (User[]) serializer.Deserialize(new StringReader(inputXml));

            context.Users.AddRange(Users);

            context.SaveChanges();

            return $"Successfully imported {Users.Length}";

        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(Product[]), new XmlRootAttribute("Products"));

            var Products =
                (Product[])serializer.Deserialize(new StringReader(inputXml));

            context.Products.AddRange(Products);

            context.SaveChanges();

            return $"Successfully imported {Products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(Category[]));

            var Categories =
                (Category[]) serializer.Deserialize(new StringReader(inputXml));

            context.Categories.AddRange(Categories);

            context.SaveChanges();

            return $"Successfully imported {Categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CategoryProduct[]));

            var CategoryProducts = (CategoryProduct[]) serializer.Deserialize(new StringReader(inputXml));

            return $"Successfully imported {CategoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var serializer = new XmlSerializer(typeof(Product[]), new XmlRootAttribute("Products"));
            var Products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                }).ToArray();

            var sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), Products, xmlNamespaces);
            return sb.ToString();
        }
    }
}