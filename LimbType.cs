using CaveGame.Enums;

namespace CaveGame;

public class LimbType
{
	private LimbType? _ancestor;
	private List<LimbCategories> _categories;
	private string _id;
	private string _name;
	private int _baseHealth;
	private int _baseHitWeight;

	public LimbType(LimbType? ancestor, List<LimbCategories>? categories, string id, string? name, int? baseHealth, int? baseHitWeight)
	{
		if (ancestor != null)
		{
			UpdateAncestor(ancestor, categories, name, baseHealth, baseHitWeight);
		}
		else
		{
			_ancestor = null;
			_categories = categories ?? throw new ArgumentNullException(nameof(categories));
			_id = id ?? throw new ArgumentNullException(nameof(id));
			_name = name ?? throw new ArgumentNullException(nameof(name));
			_baseHealth = baseHealth ?? throw new ArgumentNullException(nameof(baseHealth));
			_baseHitWeight = baseHitWeight ?? throw new ArgumentNullException(nameof(baseHitWeight));
		}
	}
	
	public void UpdateAncestor(LimbType ancestor, List<LimbCategories>? categories, string? name, int? baseHealth, int? baseHitWeight)
	{
		_ancestor = ancestor;
		_categories = categories ?? ancestor.GetCategories();
		_name = name ?? ancestor.GetName();
		_baseHealth = baseHealth ?? ancestor.GetBaseHealth();
		_baseHitWeight = baseHitWeight ?? ancestor.GetBaseHitWeight();
	}

	public List<LimbCategories> GetCategories()
	{
		return _categories;
	}
	
	public string GetId()
	{
		return _id;
	}

	public string GetName()
	{
		return _name;
	}

	public int GetBaseHealth()
	{
		return _baseHealth;
	}
	
	public int GetBaseHitWeight()
	{
		return _baseHitWeight;
	}
}