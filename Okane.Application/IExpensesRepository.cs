namespace Okane.Application;

public interface IExpensesRepository : IRepository<Expense>
{
    Expense Update(int id, UpdateExpenseRequest request, Category category);
}