namespace CaveGame;
public static class GameSettings
{
    // game settings
    public const int TURN_SPEED = 10;
    public const int CHUNK_HEIGHT = 128;
    public const int CHUNK_WIDTH = 128;
    public const int SIMPLEX_WALL_THRESHOLD = 100; // making this number smaller will make less walls as noise outputs number from 0 - 256
    public const int MIN_AREA_CHECK = 0;
    public const int LOAD_DISTANCE = 10;
    public const int MIN_SPAWN_AREA = 5000;
    
    // screen size settings
    public const int GAME_WIDTH = 240;
    public const int GAME_HEIGHT = 67;
    public const int TITLE_WIDTH = GAME_WIDTH;
    public const int TITLE_HEIGHT = 40;
    public const int STARTMENU_WIDTH = GAME_WIDTH;
    public const int STARTMENU_HEIGHT = GAME_HEIGHT - TITLE_HEIGHT;
    public const int STARTCONFIGMENU_WIDTH = 60;
    public const int STARTCONFIGMENU_HEIGHT = 20;
    public const int CUSTOMCONFIGMENU_WIDTH = 60;
    public const int CUSTOMCONFIGMENU_HEIGHT = 20;
    public const int GAMEVIEW_WIDTH = 47;
    public const int GAMEVIEW_HEIGHT = 29;
    public const int GAMELOG_WIDTH = GAME_WIDTH - GAMEVIEW_WIDTH * 4 - 1;
    public const int GAMELOG_HEIGHT = GAME_HEIGHT;
    public const int GAMELOG_MAXHEIGHT = GAMELOG_HEIGHT * 10;
}
