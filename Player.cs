using CaveGame.Managers;
using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public class Player : Entity
{
    protected override string Id => "player";
    private readonly InputHandler _inputHandler;
    
    public Player(int y, int x, int layer, InputHandler inputHandler)
    {
        Name = "Player";
        Pronouns = new []{"they", "them", "their"};
        MaxHealth = 100;
        Health = MaxHealth;
        Speed = 10;
        Position = new []{y, x};
        Layer = layer;
        Glyph = new ColoredGlyph(foreground: Color.Blue, background: Color.Black, glyph: '@');
        _inputHandler = inputHandler;
    }
    
    private TaskCompletionSource<bool>? _turnActionComplete;
    public async Task Turn()
    {
        TurnIndex += Speed;

        while (TurnIndex >= TurnSpeed)
        {
            TurnIndex -= TurnSpeed;
            
            // regen 1 health per turn
            if (Health < 100) {
                Health += 1;
            }

            _turnActionComplete = new TaskCompletionSource<bool>();
            _inputHandler.PlayerInputEnabled = true;
            await _turnActionComplete.Task;
            _inputHandler.PlayerInputEnabled = false;
            System.Console.WriteLine(Position[1] + ", " + Position[0]);
            ViewManager.UpdateView(Position[0], Position[1], Layer);
        }
    }
    public void Wait()
    {
        _turnActionComplete?.SetResult(true);
    }

    public void Move(Point direction)
    {
        var wantedPosition = new Point(Position[1] + direction.X, Position[0] + direction.Y);
        var wantedChunkPosition = GetChunkPosition(wantedPosition.Y, wantedPosition.X);
        var chunkPosition = GetChunkPosition(Position[0], Position[1]);

        if (GetTile(wantedPosition.Y, wantedPosition.X, Layer).Blocking) return;
        if (chunkPosition.Y != wantedChunkPosition.Y || chunkPosition.X != wantedChunkPosition.X)
        {
            LoadSurroundingChunks(wantedChunkPosition.Y, wantedChunkPosition.X, Layer);
        }

        Position[0] = wantedPosition.Y;
        Position[1] = wantedPosition.X;

        _turnActionComplete?.SetResult(true);
    }
}