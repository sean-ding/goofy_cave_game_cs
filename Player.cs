using CaveGame.Managers;
using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public class Player : Entity
{
    protected override string Id => "swarmer";
    public override int SpawnWeight => 10;
    private readonly InputHandler _inputHandler;
    
    public Player(int y, int x, InputHandler inputHandler)
    {
        Name = "Player";
        Pronouns = new []{"they", "them", "their"};
        MaxHealth = 100;
        Health = MaxHealth;
        Speed = 10;
        Position = new Point(y, x);
        Layer = 0;
        GlyphEntity = new SadConsole.Entities.Entity(foreground: Color.Blue, background: Color.Black, glyph: '@', zIndex: 9000) { Position = new Point(GAMEVIEW_WIDTH / 2, GAMEVIEW_HEIGHT / 2) };
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
            System.Console.WriteLine(Position.X + ", " + Position.Y);
            ViewManager.UpdateView(this);
        }
    }
    public void Wait()
    {
        _turnActionComplete?.TrySetResult(true);
    }

    private int _previousChunkY;
    private int _previousChunkX;
    public void Move(Point direction)
    {
        Point wantedPosition = new Point(Position.Y + direction.Y, Position.X + direction.X);
        var wantedLocalPosition = ToLocalPosition(wantedPosition);
        var chunkPosition = GetChunkPosition(wantedPosition);

        var silly = ToLocalPosition(wantedPosition);
        System.Console.WriteLine("Local Chunk Position: " + silly.X + ", " + silly.Y);
        
        //if (GetChunk(chunkPosition.Y, chunkPosition.X, Layer).Blocking[wantedLocalPosition.Y, wantedLocalPosition.X]) return;
        if (_previousChunkY != chunkPosition.Y)
        {
            _previousChunkY = chunkPosition.Y;
            LoadSurroundingChunks(chunkPosition.Y, chunkPosition.X, Layer);
        }
        else if (_previousChunkX != chunkPosition.X)
        {
            _previousChunkX = chunkPosition.X;
            LoadSurroundingChunks(chunkPosition.Y, chunkPosition.X, Layer);
        }
        Position = wantedPosition;
        _turnActionComplete?.TrySetResult(true);
    }
}