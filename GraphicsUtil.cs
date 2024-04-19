using SadConsole.UI;

namespace CaveGame;

public static class GraphicsUtil
{
    public static int HorCentered(ScreenSurface surface, int objectWidth)
    {
        int centerAlign = (surface.Width / 2) - (objectWidth / 2);
        return centerAlign;
    }
    public static void PrintHorCentered(ScreenSurface surface, int y, string msg, Color? foregroundColor = null, Color? backgroundColor = null)
    {
        if (foregroundColor == null) { foregroundColor = Color.White; }
        surface.Print(HorCentered(surface, msg.Length), y, msg.CreateColored(foregroundColor, backgroundColor));
    }
}