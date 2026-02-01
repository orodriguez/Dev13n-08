namespace Okane.Application;

public class InMemoryExpensesRepository : InMemoryRepository<Expense>, IExpensesRepository
{
    public Expense Update(int id, UpdateExpenseRequest request, Category category)
    {
        var existing = Entities.First(e => e.Id == id);
        
        existing.Amount = request.Amount;
        existing.Category = category;
        existing.CategoryId = category.Id;

        return existing;
    }
}