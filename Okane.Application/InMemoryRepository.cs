using System.Diagnostics.CodeAnalysis;

namespace Okane.Application;

public abstract class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private int _lastId;
    protected readonly List<T> Entities = [];

    public void Add(T entity)
    {
        entity.Id = ++_lastId;
        Entities.Add(entity);
    }

    public T? ById(int id) => 
        Entities.FirstOrDefault(x => x.Id == id);

    public IEnumerable<T> All() => Entities;

    public void Remove(int id)
    {
        var existing = Entities.First(x => x.Id == id);
        Entities.Remove(existing);
    }

    public bool Exists(int id) => 
        Entities.Any(x => x.Id == id);
}