namespace CaveGame.Managers;

public class EntityManager
{
    public List<Entity> EntityList = new();

    private List<Entity> _pendingEntityAdditions = new();
    
    private List<Entity> _pendingEntityRemovals = new();

    public void UpdateEntities()
    {
        EntityList = EntityList.Except(_pendingEntityRemovals).ToList();
        EntityList.AddRange(_pendingEntityAdditions);
        _pendingEntityAdditions.Clear();
        _pendingEntityRemovals.Clear();
    }

    public void EnterChunk(Entity entity)
    {
        _pendingEntityAdditions.Add(entity);
    }
    
    public void ExitChunk(Entity entity)
    {
        _pendingEntityRemovals.Add(entity);
    }

    public Entity? GetEntity(int positionY, int positionX)
    {
        foreach (var entity in EntityList)
        {
            if (entity.Position[0] == positionY && entity.Position[1] == positionX)
            {
                return entity;
            }
        }

        return null;
    }
}