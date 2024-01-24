namespace CaveGame;

public class Limb
{
	private LimbType _limbType;
	private Limb _parent;
	private List<Limb> _children;
	private bool _vital;
	private int _health;
}