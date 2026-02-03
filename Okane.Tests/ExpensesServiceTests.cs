using Okane.Application;

namespace Okane.Tests;

public class ExpensesServiceTests
{
    private readonly ExpensesService _service;

    public ExpensesServiceTests()
    {
        var categories = new InMemoryCategoriesRepository();
        categories.Add(new Category { Name = "Food" });
        categories.Add(new Category { Name = "Drinks" });
        
        _service = new ExpensesService(new InMemoryExpensesRepository(), categories,
            new ExpenseResponseFactory());
    }

    [Fact]
    public void Create_Response()
    {
        var expense = _service.Create(new(10, "Food")).AssertOk();

        Assert.Equal(10, expense.Amount);
        Assert.Equal("Food", expense.CategoryName);
    }
    
    [Fact]
    public void Create_WithDescription_Response()
    {
        var expense = _service.Create(new(
            Amount: 10, 
            CategoryName: "Food", 
            Description: "McDonalds")
        ).AssertOk();

        Assert.Equal(10, expense.Amount);
        Assert.Equal("Food", expense.CategoryName);
        Assert.Equal("McDonalds", expense.Description);
    }

    [Fact]
    public void Create_InvalidAmount()
    {
        var message = _service.Create(new(-1, "Food")).AssertError();
        Assert.Equal("Amount must be greater than 1.", message);
    }

    [Fact]
    public void Retrieve_ExistingExpense()
    {
        var created = _service.Create(new(
            Amount: 10, 
            CategoryName: "Food", 
            Description: "McDonalds")
        ).AssertOk();

        var retrieved = _service.Retrieve(created.Id).AssertOk();

        Assert.Equal(10, retrieved.Amount);
        Assert.Equal("Food", retrieved.CategoryName);
        Assert.Equal("McDonalds", retrieved.Description);
    }

    [Fact]
    public void Retrieve_NotFound()
    {
        var message = _service.Retrieve(999).AssertNotFound();
        Assert.Contains("not found", message);
    }

    [Fact]
    public void All_ReturnsAllExpenses()
    {
        _service.Create(new(10, "Food")).AssertOk();
        _service.Create(new(20, "Drinks")).AssertOk();

        var all = _service.All().AssertOk().ToArray();

        Assert.Equal(2, all.Length);
        Assert.Contains(all, e => e.Amount == 10 && e.CategoryName == "Food");
        Assert.Contains(all, e => e.Amount == 20 && e.CategoryName == "Drinks");
    }

    [Fact(Skip = "Not implemented")]
    public void Update_CategoryNameDoesNotExist()
    {
    }
    
    [Fact]
    public void Update_ExistingExpense()
    {
        var created = _service.Create(new(10, "Food")).AssertOk();

        var updated = _service.Update(created.Id, new(20, "Drinks")).AssertOk();

        Assert.Equal(20, updated.Amount);
        Assert.Equal("Drinks", updated.CategoryName);
    }

    [Fact]
    public void Update_InvalidAmount()
    {
        var message = _service.Update(1, new(-5, "Food")).AssertError();
        Assert.Equal("Amount must be greater than 1.", message);
    }

    [Fact]
    public void Update_NotFound()
    {
        var message = _service.Update(999, new(20, "Drinks")).AssertNotFound();
        Assert.Contains("not found", message);
    }

    [Fact]
    public void Delete_ExistingExpense()
    {
        var created = _service.Create(new(10, "Food")).AssertOk();

        _service.Delete(created.Id).AssertOk();
    }

    [Fact]
    public void Delete_NotFound()
    {
        var message = _service.Delete(999).AssertNotFound();
        Assert.Contains("not found", message);
    }
}