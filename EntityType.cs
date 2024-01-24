using System.Text.Json.Serialization;

namespace CaveGame;

public class EntityType
{
	private static Dictionary<string, EntityType> _loadedEntityTypes;

	[JsonInclude]
	public EntityType? Ancestor { get; private set; } 
	[JsonInclude]
	public List<Limb> Limbs { get; private set; }
	[JsonInclude]
	public string Id { get; private set; }
	[JsonInclude]
	public string Name { get; private set; }
	[JsonInclude]
	public string[] Pronouns { get; private set; }
	[JsonInclude]
	public int BaseLevel { get; private set; }
	[JsonInclude]
	public int BaseSpeed { get; private set; }
	[JsonInclude]
	public ColoredGlyph Glyph { get; private set; }
	
	public EntityType(EntityType? ancestor, List<Limb>? limbs, string id, string? name, string[]? pronouns, int? baseLevel, int? baseSpeed, ColoredGlyph? glyph)
	{
		if (ancestor != null)
		{
			UpdateAncestor(ancestor, limbs, name, pronouns, baseLevel, baseSpeed, glyph);
		}
		else
		{
			Ancestor = null;
			_limbs = limbs ?? throw new ArgumentNullException(nameof(limbs));
			_name = name ?? throw new ArgumentNullException(nameof(name));
			_pronouns = pronouns ?? throw new ArgumentNullException(nameof(pronouns));
			_baseLevel = baseLevel ?? throw new ArgumentNullException(nameof(baseLevel));
			_baseSpeed = baseSpeed ?? throw new ArgumentNullException(nameof(baseSpeed));
			_glyph = glyph ?? throw new ArgumentNullException(nameof(glyph));
		}

		Id = id;
	}

	public void UpdateAncestor(EntityType ancestor, List<Limb>? limbs, string? name, string[]? pronouns, int? baseLevel, int? baseSpeed, ColoredGlyph? glyph)
	{
		Ancestor = ancestor;
		Limbs = limbs ?? ancestor.GetLimbs();
		_name = name ?? ancestor.GetName();
		_pronouns = pronouns ?? ancestor.GetPronouns();
		_baseLevel = baseLevel ?? ancestor.GetBaseLevel();
		_baseSpeed = baseSpeed ?? ancestor.GetBaseSpeed();
		_glyph = glyph ?? ancestor.GetGlyph();
	}

	public Entity CreateEntity(int y, int x, int layer)
	{
		return new Entity(this, y, x, layer);
	}

	public List<Limb> GetLimbs()
	{
		return Limbs;
	}

	public string GetId()
	{
		return Id;
	}

	public string GetName()
	{
		return _name;
	}

	public string[] GetPronouns()
	{
		return _pronouns;
	}

	public int GetBaseLevel()
	{
		return _baseLevel;
	}

	public int GetBaseSpeed()
	{
		return _baseSpeed;
	}

	public ColoredGlyph GetGlyph()
	{
		return _glyph;
	}
}