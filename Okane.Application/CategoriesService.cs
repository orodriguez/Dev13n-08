namespace Okane.Application;

public class CategoriesService(ICategoriesRepository categories)
{
    public Result<CategoryResponse> Create(CreateCategoryRequest request)
    {
        // test por si ya existen
        if(categories.ByName(request.Name) != null)
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

        if (category is null)
            return new NotFoundResult(
                $"{nameof(Category)} with id {createdId} was not found.");

        if (category.Expenses.Any())
            return new ErrorResult(
                "Can not delete category with existing expenses");

        categories.Remove(createdId);

        return new OkResult();
    }

    public Result<CategoryResponse> Update(int id, UpdateCategoryRequest request)
    {
        var existing = categories.ById(id);

        if (existing is null)
            return new NotFoundResult<CategoryResponse>(
                $"{nameof(Category)} with id {id} was not found.");

        var duplicate = categories.ByName(request.Name);

        if (duplicate != null && duplicate.Id != id)
            return new ErrorResult<CategoryResponse>("Category already exists");

        existing.Name = request.Name;

        return new OkResult<CategoryResponse>(
            new(existing.Id, existing.Name));
    }
}