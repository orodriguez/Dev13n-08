using Okane.Application;

namespace Okane.Tests;

public class CategoriesServiceTests
{
    private readonly CategoriesService _service;

    public CategoriesServiceTests()
    {
        _service = new CategoriesService(new InMemoryCategoriesRepository());
    }
    
    [Fact]
    public void Create_Response()
    {
        var category = _service.Create(new CreateCategoryRequest("Food")).AssertOk();

        Assert.Equal(1, category.Id);
        Assert.Equal("Food", category.Name);
    }
    
    [Fact]
    public void Retrieve_ExistingCategory()
    {
        var created = _service.Create(new CreateCategoryRequest("Drinks")).AssertOk();

        var retrieved = _service.Retrieve(created.Id).AssertOk();

        Assert.Equal(1, retrieved.Id);
        Assert.Equal("Drinks", retrieved.Name);
    }
    
    [Fact]
    public void Retrieve_NotFound()
    {
        var message = _service.Retrieve(999).AssertNotFound();
        Assert.Contains("not found", message);
    }
    
    [Fact]
    public void All()
    {
        _service.Create(new CreateCategoryRequest("Food")).AssertOk();
        _service.Create(new CreateCategoryRequest("Drinks")).AssertOk();

        var all = _service.All().AssertOk().ToArray();

        Assert.Equal(2, all.Length);
        Assert.Contains(all, e => e.Name == "Food");
        Assert.Contains(all, e => e.Name == "Drinks");
    }
}