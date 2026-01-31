namespace Okane.Application;

public class CategoriesService
{
    private readonly ICategoriesRepository categories;
    private readonly IExpensesRepository expenses;
    public CategoriesService(ICategoriesRepository categories, IExpensesRepository expenses)
    {
        this.categories = categories;
        this.expenses = expenses;
    }
    public Result<CategoryResponse> Create(CreateCategoryRequest request)
    {
        var existing = categories.ByName(request.Name);
        if(existing != null)
            return new ErrorResult<CategoryResponse>($"Category already exists");
        
        
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
        if(!categories.Exists(createdId))
            return new ErrorResult($"Can not delete category with existing expenses");
        
        if(expenses.AnyByCategoryId(createdId))
            return new ErrorResult($"Can not delete category with existing expenses");
        
        categories.Remove(createdId);
        return new OkResult();
    }

    public Result<CategoryResponse> Update(int id, UpdateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return new ErrorResult<CategoryResponse>("Name can not be empty.");
        
        if (!categories.Exists(id)) return new NotFoundResult<CategoryResponse>("The categorie was not found.");

        var existing = categories.ByName(request.Name);
        
        if (existing != null && existing.Id != id)
            return new ErrorResult<CategoryResponse>($"Category already exists.");
        
        var updated = categories.Update(id, request);
        
        var response = new CategoryResponse(updated.Id, updated.Name);
        
        return new OkResult<CategoryResponse>(response);
    }
}