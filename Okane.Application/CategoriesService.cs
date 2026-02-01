namespace Okane.Application;

public class CategoriesService(ICategoriesRepository categories, IExpensesRepository expenses)
{
    public Result<CategoryResponse> Create(CreateCategoryRequest request)
    {
        var existing = categories.ByName(request.Name);

        if (existing != null)
            return new ErrorResult<CategoryResponse>("Category already exists");
        
        var category = new Category
        {
            Name = request.Name
        };
        categories.Add(category);
        
        return new OkResult<CategoryResponse>(new CategoryResponse(category.Id, category.Name));
    }

    public Result<CategoryResponse> Retrieve(int id)
    {
        var category = categories.ById(id);

        if (category == null)
            return new NotFoundResult<CategoryResponse>($"{nameof(Category)} with id {id} was not found.");
        
        return new OkResult<CategoryResponse>(new CategoryResponse(category.Id, category.Name));
    }

    public Result<IEnumerable<CategoryResponse>> All()
    {
        var response = categories
            .All()
            .Select(c => new CategoryResponse(c.Id, c.Name));
        
        return new OkResult<IEnumerable<CategoryResponse>>(response);
    }
    
    public Result Remove(int createdId)
    {
        var category = categories.ById(createdId);

        if (category == null)
            return new NotFoundResult($"{nameof(Category)} with id {createdId} was not found.");
        
        var hasExpenses = expenses.All().Any(e => e.CategoryId == createdId);
        
        if (hasExpenses)
            return new ErrorResult("Can not delete category with existing expenses");

        categories.Remove(category.Id);

        return new OkResult();
    }

    public Result<CategoryResponse> Update(int id, UpdateCategoryRequest request)
    {
        var category = categories.ById(id);

        if (category == null)
            return new NotFoundResult<CategoryResponse>(
                $"{nameof(Category)} with id {id} was not found."
            );

        var existing = categories.ByName(request.Name);

        if (existing != null && existing.Id != id)
            return new ErrorResult<CategoryResponse>("Category already exists");

        category.Name = request.Name;

        return new OkResult<CategoryResponse>(
            new CategoryResponse(category.Id, category.Name)
        );
    }
}