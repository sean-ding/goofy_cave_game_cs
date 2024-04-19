using SadConsole.Ansi;
using SadConsole.Instructions;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class ExitScreen : ScreenObject
{
    private ScreenSurface _exit;
    public ExitScreen()
    {
        // create a screen for exit art
        _exit = new ScreenSurface(80, 25) { Position = new Point(GAME_WIDTH / 4 - 40, GAME_HEIGHT / 4 - 12), FontSize = Game.Instance.DefaultFont.GetFontSize(IFont.Sizes.Two) };
        
        var doc = new Document($"Art/god.ans");
        var writer = new AnsiWriter(doc, _exit.Surface);
        writer.ReadEntireDocument();
        _exit.IsVisible = false;
        Children.Add(_exit);

        var exitAnim = new InstructionSet()

            .Wait(TimeSpan.FromSeconds(1.0d))

            .Code((host, delta) =>
            {
                _exit.IsVisible = true;
                return true;
            })

            .Instruct(new FadeTextSurfaceTint(_exit,
                new Gradient(Color.Transparent, Color.Black),
                TimeSpan.FromSeconds(2.5d)))

            .Wait(TimeSpan.FromSeconds(1.0d))
            
            .Code((host, delta) =>
            {
                Environment.Exit(0);
                return true;
            });
        
        exitAnim.RemoveOnFinished = true;
        
        SadComponents.Add(exitAnim);
    }
}