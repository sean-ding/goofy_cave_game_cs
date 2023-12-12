namespace CaveGame.Managers;

public class EntityManager
{
    private List<Guid> _entityList = new();
    private Dictionary<Type, Dictionary<Guid, IComponent>> _componentPools = new();

    public Guid CreateEntity(IComponent[]? components = null)
    {
        var guid = Guid.NewGuid();

        return guid;
    }

    public void AddComponent(Guid entity, IComponent component)
    {
        if (_componentPools.TryGetValue(component.GetType(), out var pool))
        {
            pool.Add(entity, component);
        }
        else
        {
            pool = new Dictionary<Guid, IComponent>();
            _componentPools.Add(component.GetType(), pool);
        }
    }
}