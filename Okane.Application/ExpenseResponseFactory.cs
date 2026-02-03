namespace Okane.Application;

public class ExpenseResponseFactory
{
    public ExpenseResponse Create(Expense expense) =>
        new(
            expense.Id,
            expense.Amount,
            expense.CategoryName,
            expense.Description);
}