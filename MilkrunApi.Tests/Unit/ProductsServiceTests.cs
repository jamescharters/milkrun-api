using AutoMapper;
using MilkrunApi.Exceptions;
using MilkrunApi.Mapping;
using MilkrunApi.Models;
using MilkrunApi.Models.Entity;
using MilkrunApi.Repositories;
using MilkrunApi.Services;
using Moq;
using NUnit.Framework;

namespace MilkrunApi.Tests.Unit;

public class ProductsServiceTests
{
    private IMapper _mapper;
    private Mock<IProductsRepository> _mockedRepository;

    [SetUp]
    public void Setup()
    {
        _mockedRepository = new Mock<IProductsRepository>();
        _mapper = new Mapper(new MapperConfiguration(mapperConfigurationExpression =>
        {
            mapperConfigurationExpression.AddProfile<MappingProfile>();
        }));
    }

    [TearDown]
    public void TearDown()
    {
        _mockedRepository.Reset();
    }

    [Test]
    public void CreateAsync_Throws_ProductExistsException()
    {
        _mockedRepository.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _mockedRepository.Setup(r => r.CreateAsync(It.IsAny<ProductRequest>()))
            .ReturnsAsync(new ProductEntity());

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        Assert.ThrowsAsync<DuplicateProductException>(async () => await service.CreateAsync(new ProductRequest
        {
            Title = "Fake Title",
            Brand = "Fake Brand",
            Description = "Fake Description",
            Price = 0
        }));
        _mockedRepository.Verify(r => r.CreateAsync(It.IsAny<ProductRequest>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_Calls_Repository()
    {
        _mockedRepository.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        _mockedRepository.Setup(r => r.CreateAsync(It.IsAny<ProductRequest>()))
            .ReturnsAsync(new ProductEntity());

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        await service.CreateAsync(new ProductRequest
        {
            Title = "Title",
            Brand = "Brand",
            Description = "Description",
            Price = 0
        });

        _mockedRepository.Verify(r => r.CreateAsync(It.IsAny<ProductRequest>()), Times.Once);
    }

    [Test]
    public void UpdateAsync_Throws_DuplicateProductException()
    {
        // Throw an exception if we attempt to update an existing product to a Title and Brand that matches another 
        // (i.e. different Id) product in the database
        
        _mockedRepository.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);
        _mockedRepository.Setup(r => r.OtherProductExistsAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _mockedRepository.Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<ProductRequest>()));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        Assert.ThrowsAsync<DuplicateProductException>(async () => await service.UpdateAsync(1, new ProductRequest
        {
            Title = "Fake Title",
            Brand = "Fake Brand",
            Description = "Fake Description",
            Price = 0
        }));

        _mockedRepository.Verify(r => r.CreateAsync(It.IsAny<ProductRequest>()), Times.Never);
    }

    [Test]
    public void UpdateAsync_Throws_InvalidProductException()
    {
        _mockedRepository.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(false);
        _mockedRepository.Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<ProductRequest>()));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        Assert.ThrowsAsync<InvalidProductException>(async () => await service.UpdateAsync(1, new ProductRequest
        {
            Title = "Fake Title",
            Brand = "Fake Brand",
            Description = "Fake Description",
            Price = 0
        }));

        _mockedRepository.Verify(r => r.CreateAsync(It.IsAny<ProductRequest>()), Times.Never);
    }

    [Test]
    public async Task UpdateAsync_Calls_Repository()
    {
        _mockedRepository.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);
        _mockedRepository.Setup(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<ProductRequest>()));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        await service.UpdateAsync(1, new ProductRequest
        {
            Title = "Fake Title",
            Brand = "Fake Brand",
            Description = "Fake Description",
            Price = 0
        });

        _mockedRepository.Verify(r => r.UpdateAsync(It.IsAny<long>(), It.IsAny<ProductRequest>()), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_Throws_ArgumentOutOfRangeException_Page()
    {
        _mockedRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new Tuple<IEnumerable<ProductEntity>, int>(new List<ProductEntity>(), 0));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await service.GetAllAsync(-1000));

        _mockedRepository.Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task GetAllAsync_Throws_ArgumentOutOfRangeException_PageSize()
    {
        _mockedRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new Tuple<IEnumerable<ProductEntity>, int>(new List<ProductEntity>(), 0));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await service.GetAllAsync(1, -1000));

        _mockedRepository.Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task GetAllAsync_Calls_Repository()
    {
        _mockedRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new Tuple<IEnumerable<ProductEntity>, int>(new List<ProductEntity>(), 0));

        var service = new ProductsService(_mockedRepository.Object, _mapper);

        await service.GetAllAsync(1);

        _mockedRepository.Verify(r => r.GetAllAsync(1, 10), Times.Once);
    }
}