namespace Okane.Application;

public class Expense : IEntity
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public required Category Category { get; set; }
    public string CategoryName => Category.Name;
}