namespace Okane.Application;

public class InMemoryExpensesRepository : InMemoryRepository<Expense>, IExpensesRepository
{
    public Expense Update(int id, UpdateExpenseRequest request, Category category)
    {
        var existing = Entities.FirstOrDefault(e => e.Id == id);
        if (existing == null) 
            throw new InvalidOperationException($"Expense with id {id} not found.");
        
        existing.Amount = request.Amount;
        existing.Category = category;

        return existing;
    }
    public bool AnyByCategoryId(int categoryId)
    {
        return Entities.Any(e => e.Category.Id == categoryId);
    }
}