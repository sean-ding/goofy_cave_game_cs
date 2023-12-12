using SadConsole.Input;
using static CaveGame.Program;

namespace CaveGame;

public class InputHandler
{
    public bool PlayerInputEnabled = false;

    public void Input(Keys key)
    {
        if (!PlayerInputEnabled) return;
        
        var player = GetPlayer();
        switch (key)
        {
            case Keys.W:
                player.Move(new Point(0, -1));
                break;
            case Keys.S:
                player.Move(new Point(0, 1));
                break;
            case Keys.A:
                player.Move(new Point(-1, 0));
                break;
            case Keys.D:
                player.Move(new Point(1, 0));
                break;
            case Keys.Space:
                player.Wait();
                break;
        }
    }
}