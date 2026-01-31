namespace Okane.Application;

public class InMemoryCategoriesRepository : InMemoryRepository<Category>, ICategoriesRepository
{
    public Category? ByName(string name) => 
        Entities.FirstOrDefault(category => category.Name == name);

    public Category Update(int id, UpdateCategoryRequest request)
    {
        var existing = Entities.FirstOrDefault(e => e.Id  == id);

        if (existing == null)
            throw new InvalidOperationException($"Category with id {id} not found.");
        existing.Name = request.Name;
        
        return existing;
    }
}