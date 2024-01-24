using SadConsole.Input;
using static CaveGame.Program;

namespace CaveGame;

public class InputHandler
{
	public bool PlayerInputEnabled = false;

	public void Input(Keys key)
	{
		if (!PlayerInputEnabled) return;

	}
}