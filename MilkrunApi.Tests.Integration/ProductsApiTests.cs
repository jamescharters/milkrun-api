using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MilkrunApi.Models.DTO;
using MilkrunApi.Models.Entity;
using Newtonsoft.Json;

namespace MilkrunApi.Tests.Integration;

public class ProductsApiTests
{
    private const string API_URL = "/api/v1/Products";

    private readonly ProductRequest CreateProductRequest = new()
    {
        Brand = "Fake Brand",
        Category = "Fake Category",
        Description = "Fake Description",
        Images = new List<string>(new[] { "http://foo.bar/image.png" }),
        Price = 123,
        Rating = 4.25M,
        Stock = 1000,
        Thumbnail = "http://foo.bar/thumbnail.png",
        Title = "Fake Title",
        DiscountPercentage = 0.5M
    };

    private CustomWebApplicationFactory<Program> _factory;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();
    }

    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }

    private HttpClient CreateClient(bool withAuth = false)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        if (withAuth)
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String("test_user:test_password"u8.ToArray()));

        return client;
    }

    [Test]
    public async Task Get_Products_Should_Return_First_Page()
    {
        var response = await CreateClient().GetAsync(API_URL);

        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<ProductsCollection>(await response.Content.ReadAsStringAsync());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(content.Total, Is.EqualTo(30));
        Assert.That(content.Skip, Is.EqualTo(0));
        Assert.That(content.Limit, Is.EqualTo(10));
        Assert.That(content.Products.Count(), Is.EqualTo(10));
    }

    [Test]
    public async Task Get_Products_Should_Return_Different_Page_And_Limit()
    {
        var response = await CreateClient().GetAsync($"{API_URL}?page=2&limit=5");

        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<ProductsCollection>(await response.Content.ReadAsStringAsync());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(content.Total, Is.EqualTo(30));
        Assert.That(content.Skip, Is.EqualTo(2));
        Assert.That(content.Limit, Is.EqualTo(5));
        Assert.That(content.Products.Count(), Is.EqualTo(5));
    }
    
    [Test]
    public async Task Get_Products_Should_Return_Large_ResultSet()
    {
        var response = await CreateClient().GetAsync($"{API_URL}?page=0&limit=30");

        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<ProductsCollection>(await response.Content.ReadAsStringAsync());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(content.Total, Is.EqualTo(30));
        Assert.That(content.Skip, Is.EqualTo(0));
        Assert.That(content.Limit, Is.EqualTo(30));
        Assert.That(content.Products.Count(), Is.EqualTo(30));
    }

    [Test]
    public async Task Create_Product_Should_Succeed_If_Authenticated()
    {
        var authenticatedClient = CreateClient(true);

        var response = await authenticatedClient.PostAsJsonAsync(API_URL, CreateProductRequest);

        response.EnsureSuccessStatusCode();

        var createdProduct = JsonConvert.DeserializeObject<ProductEntity>(await response.Content.ReadAsStringAsync());

        Assert.That(createdProduct.Id > 0);
        Assert.That(createdProduct.Title, Is.EqualTo(CreateProductRequest.Title));
        Assert.That(createdProduct.Price, Is.EqualTo(CreateProductRequest.Price));
        Assert.That(createdProduct.Description, Is.EqualTo(CreateProductRequest.Description));
        Assert.That(createdProduct.Brand, Is.EqualTo(CreateProductRequest.Brand));
        Assert.That(createdProduct.Category, Is.EqualTo(CreateProductRequest.Category));
        Assert.That(createdProduct.Images, Is.EqualTo(CreateProductRequest.Images));
        Assert.That(createdProduct.Rating, Is.EqualTo(CreateProductRequest.Rating));
        Assert.That(createdProduct.Stock, Is.EqualTo(CreateProductRequest.Stock));
        Assert.That(createdProduct.Thumbnail, Is.EqualTo(CreateProductRequest.Thumbnail));
        Assert.That(createdProduct.DiscountPercentage, Is.EqualTo(CreateProductRequest.DiscountPercentage));
    }

    [Test]
    public async Task Create_Product_Should_Conflict_If_Title_And_Brand_Are_Duplicates()
    {
        var authenticatedClient = CreateClient(true);

        var firstCreateResponse = await authenticatedClient.PostAsJsonAsync(API_URL, CreateProductRequest);

        firstCreateResponse.EnsureSuccessStatusCode();

        var secondCreateResponse = await authenticatedClient.PostAsJsonAsync(API_URL, new ProductRequest
        {
            Title = CreateProductRequest.Title,
            Brand = CreateProductRequest.Brand,
            Price = 123,
            Description = "Anything Else"
        });

        Assert.That(secondCreateResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task Create_Product_Should_Fail_If_Unauthenticated()
    {
        var response = await CreateClient().PostAsJsonAsync(API_URL, CreateProductRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Update_Product_Should_Succeed_If_Authenticated()
    {
        var authenticatedClient = CreateClient(true);

        // Create 
        var createResponse = await authenticatedClient.PostAsJsonAsync(API_URL, CreateProductRequest);

        var createdProduct =
            JsonConvert.DeserializeObject<ProductEntity>(await createResponse.Content.ReadAsStringAsync());

        // Update
        var updateResponse = await authenticatedClient.PutAsJsonAsync($"{API_URL}/{createdProduct.Id}",
            new ProductRequest
            {
                Title = "Modified Title",
                Brand = "Modifed Brand",
                Description = "Modified Description",
                Price = 256
            });

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Update_Product_Should_Conflict_If_Title_And_Brand_Are_Duplicates()
    {
        var authenticatedClient = CreateClient(true);

        await authenticatedClient.PostAsJsonAsync(API_URL, CreateProductRequest);

        // Try to update another product, but with Title and Brand matching that which we just created
        var updateResponse = await authenticatedClient.PutAsJsonAsync($"{API_URL}/1",
            new ProductRequest
            {
                Title = CreateProductRequest.Title,
                Description = "Modified Description",
                Brand = CreateProductRequest.Brand,
                Price = 256
            });

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task Update_Product_Should_NotFound_If_Product_Does_Not_Exist()
    {
        var authenticatedClient = CreateClient(true);

        var updateResponse = await authenticatedClient.PutAsJsonAsync($"{API_URL}/123", new ProductRequest
        {
            Title = CreateProductRequest.Title,
            Description = "Modified Description",
            Brand = CreateProductRequest.Brand,
            Price = 256
        });

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Update_Product_Should_Fail_If_Already_Exists()
    {
        var authenticatedClient = CreateClient(true);

        // Create 
        var createResponse = await authenticatedClient.PostAsJsonAsync(API_URL, CreateProductRequest);

        var createdProduct =
            JsonConvert.DeserializeObject<ProductEntity>(await createResponse.Content.ReadAsStringAsync());

        // Update
        var updateResponse = await authenticatedClient.PutAsJsonAsync($"{API_URL}/{createdProduct.Id}",
            new ProductRequest
            {
                Title = "Title",
                Description = "Description",
                Brand = "Brand",
                Price = 256
            });

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Update_Product_Should_Fail_If_Unauthenticated()
    {
        var authenticatedClient = CreateClient();

        var updateResponse = await authenticatedClient.PutAsJsonAsync($"{API_URL}/1", new ProductRequest
        {
            Title = "Modified Title",
            Description = "Modified Description",
            Price = 256
        });

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}