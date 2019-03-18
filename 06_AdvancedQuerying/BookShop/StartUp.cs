using System;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {}

        //Task 0
        public static void Seed()
        {
            using (var db = new BookShopContext())
            {
                DbInitializer.ResetDatabase(db);
            }
        }

        //Task 1
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(a => a.AgeRestriction == ageRestriction)
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList();
            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        //Task 2
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(e => e.EditionType.ToString() == "Gold" && e.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(t => t.Title)
                .ToList();
            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        //Task 3
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new {
                    Title = b.Title,
                    Price = b.Price
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 4
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new {
                    Title = b.Title,
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 5
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split().Select(t => t.ToLower()).ToArray();
            StringBuilder sb = new StringBuilder();
            context.Books
                .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(t => t.Title)
                .OrderBy(x => x)
                .ToList()
                .ForEach(x => sb.AppendLine(x));

            return sb.ToString().TrimEnd('\r', '\n');

        }

        //Task 6
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var Date = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var sb = new StringBuilder();

            context.Books
                .Where(b => DateTime.Compare(b.ReleaseDate.Value, Date) == -1)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Type = b.EditionType.ToString(),
                    Price = b.Price
                })
                .ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - {b.Type} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 7
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 8
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            input = input.ToLower();

            context.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd('\r', '\n');

        }

        //Task 9
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            input = input.ToLower();

            context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToList()
                .ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 10
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(b => b.Title.Length > lengthCheck).ToList().Count;
        }

        //Task 11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Authors
                .Where(a => a.Books.Count != 0)
                .OrderByDescending(a => a.Books.Sum(b => b.Copies))
                .Select(a => $"{a.FirstName} {a.LastName} - {a.Books.Sum(b => b.Copies)}")
                .ToList()
                .ForEach(a => sb.AppendLine(a));
            
            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();
            context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.CategoryName} ${c.Profit:F2}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Categories
                .Select(c => new
                {
                    Name = $"--{c.Name}",
                    Books = c.CategoryBooks.OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3)
                        .Select(cb => $"{cb.Book.Title} ({cb.Book.ReleaseDate.Value.Year})")
                        .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.Name}{Environment.NewLine}{string.Join(Environment.NewLine, c.Books)}"));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 14
        public static string IncreasePrices(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd('\r', '\n');
        }

        //Task 15
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200);
            int booksCount = books.Count();
            context.Books.RemoveRange(books);
            context.SaveChanges();
            return (booksCount);
        }
    }
}
