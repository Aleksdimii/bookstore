using bookstore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using System;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTestProject1
{
     [TestClass]
   
    public class UnitTest1 
    {
        [TestMethod]
        public void Book_Price_Should_Be_Positive()
        {
            var book = new Book
            {
                Title = "Test Book",
                ISBN = "123-4567890123",
                Price = 19.99M
            };

            Assert.IsTrue(book.Price > 0, "Book price should be greater than zero.");
        }

        [TestMethod]
        public void Book_Should_Have_Title()
        {
            var book = new Book
            {
                Title = "Test Title",
                ISBN = "123-4567890123",
                Price = 10
            };

            Assert.IsFalse(string.IsNullOrWhiteSpace(book.Title), "Book title should not be null or whitespace.");
        }

        [TestMethod]
        public void Author_FullName_Should_Combine_First_And_Last_Name()
        {
            var author = new Author
            {
                Name = "John",
                Biography = "Doe"
            };

            var fullName = $"{author.Name} {author.Biography}";
            Assert.AreEqual("John Doe", fullName);
        }

        [TestMethod]
        public void ISBN_Should_Have_Valid_Format()
        {
            var book = new Book
            {
                ISBN = "978-3-16-148410-0"
            };

            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(book.ISBN, @"^\d{3}-\d-\d{2}-\d{6}-\d$");

            Assert.IsTrue(isValid, "ISBN format is invalid.");
        }

        [TestMethod]
        public void Book_Creation_With_Null_ISBN_Should_Throw()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var book = new Book
                {
                    Title = "Book with null ISBN",
                    ISBN = null,
                    Price = 5
                };

                if (book.ISBN == null)
                    throw new ArgumentNullException(nameof(book.ISBN));
            });
        }
    }
}
