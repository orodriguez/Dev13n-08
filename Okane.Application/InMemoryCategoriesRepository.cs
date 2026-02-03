namespace Okane.Application;

public class InMemoryCategoriesRepository : InMemoryRepository<Category>, ICategoriesRepository
{
    public Category ByName(string name) => 
        Entities.First(category => category.Name == name);
}