using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Tests.API.Models;
using Tests.Core.Fixtures;

namespace Tests.API.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("API")]
[AllureFeature("Bookstore")]
public class BookstoreApiTests : ApiTestFixture
{
    [Test]
    [AllureTag("Smoke")]
    [AllureName("GET /Books returns non-empty list")]
    public async Task GetBooks_ReturnsBooks()
    {
        var response = await Api.GetAsync<BooksResponse>("BookStore/v1/Books");

        Assert.That(response.Books, Is.Not.Empty);
    }

    [Test]
    [AllureName("GET /Books returns books with titles")]
    public async Task GetBooks_AllBooksHaveTitles()
    {
        var response = await Api.GetAsync<BooksResponse>("BookStore/v1/Books");

        Assert.That(response.Books, Has.All.Property(nameof(Book.Title)).Not.Empty);
    }

    [Test]
    [AllureName("GET /Book by ISBN returns correct book")]
    public async Task GetBook_ByIsbn_ReturnsMatchingBook()
    {
        var allBooks = await Api.GetAsync<BooksResponse>("BookStore/v1/Books");
        var firstIsbn = allBooks.Books.First().Isbn;

        var book = await Api.GetAsync<Book>($"BookStore/v1/Book?ISBN={firstIsbn}");

        Assert.That(book.Isbn, Is.EqualTo(firstIsbn));
    }
}
