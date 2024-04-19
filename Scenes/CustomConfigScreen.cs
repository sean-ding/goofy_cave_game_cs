using CaveGame.Generation;
using SadConsole.UI;
using SadConsole.UI.Controls;
using static CaveGame.GraphicsUtil;
using static CaveGame.GameSettings;

namespace CaveGame.Scenes;

public class CustomConfigScreen : ScreenObject
{
    private ScreenSurface _customConfig;
    private Console _customConfigMenu;

    public CustomConfigScreen()
    {
        // create a screen for config background
        _customConfig = new ScreenSurface(GAME_WIDTH, GAME_HEIGHT);
        // add config background
        Children.Add(_customConfig);
        
        // create a config menu
        _customConfigMenu = new CustomConfigMenu() { Position = new Point(HorCentered(_customConfig, CUSTOMCONFIGMENU_WIDTH), 2) };
        // add config menu
        Children.Add(_customConfigMenu);
        
        // create menu border
        var borderParams = Border.BorderParameters.GetDefault()
            .AddTitle("Custom Configuration", Color.DarkCyan, Color.Transparent)
            .ChangeBorderColors(Color.White, Color.Black)
            .AddShadow();

        // add menu border
        Border border = new(_customConfigMenu, borderParams);
    }
    public class CustomConfigMenu : ControlsConsole
    {
        public CustomConfigMenu() : base(CUSTOMCONFIGMENU_WIDTH, CUSTOMCONFIGMENU_HEIGHT)
        {
            PrintHorCentered(this, 1, "Chunk Size:");
            PrintHorCentered(this, 2, "(128 recommended)");
            var chunkSize = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 3)
            };
            PrintHorCentered(this, 5, "Minimum Chunk Area:");
            PrintHorCentered(this, 6, "(8000 recommended, ignored if seed is set)");
            var minChunkArea = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 7)
            };
            PrintHorCentered(this, 9, "Seed:");
            PrintHorCentered(this, 10, "(leave empty for random)");
            var seed = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 11)
            };
            PrintHorCentered(this, 13, "Number of Enemies:");
            var enemyCount = new TextBox(10)
            {
                Position = new Point(HorCentered(this, 10), 14)
            };
            var confirmButton = new Button(20)
            {
                Text = "Generate Chunk",
                Position = new Point(HorCentered(this, 20), 18)
            };
            confirmButton.Click += (_, _) =>
            {
                this.Erase(0, 16, Width);
                if (int.TryParse(chunkSize.Text, out var i) && int.TryParse(minChunkArea.Text, out var j) && int.TryParse(enemyCount.Text, out var k))
                {
                    // check if Chunk size can't be larger than min area
                    if (i * i / 2 < j)
                    {
                        PrintHorCentered(this, 16, "Max minimum area is half of size squared!", Color.Red);
                        return;
                    }

                    // checks if seed is empty
                    // also enemyCount is unused currently
                    if (seed.Text == "")
                    {
                        // TODO: add code to call some function in program script to generate world and player
                    }
                    else if (int.TryParse(seed.Text, out var l))
                    {
                        // TODO: add code to call some function in program script to generate world and player
                    }
                    else
                    {
                        PrintHorCentered(this, 16, "Inputs must be integers!", Color.Red);
                    }
                }
                else
                {
                    PrintHorCentered(this, 16, "Inputs must be integers!", Color.Red);
                }
            };
            
            Controls.Add(chunkSize);
            Controls.Add(minChunkArea);
            Controls.Add(seed);
            Controls.Add(enemyCount);
            Controls.Add(confirmButton);
        }
    }
}