namespace CaveGame;

public class Entity
{
	private EntityType _entityType;
	private List<Limb> _limbs;
	private int _level;
	private int _speed;
	private int[] _position;
	private int _layer;

	public Entity(EntityType entityType, int y, int x, int layer)
	{
		_entityType = entityType;
		_limbs = entityType.GetLimbs();
		_level = entityType.GetBaseLevel();
		_speed = entityType.GetBaseSpeed();
		_position = new[] { y, x };
		_layer = layer;
	}
}