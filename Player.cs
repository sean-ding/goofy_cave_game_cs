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
            ViewManager.UpdateView(this);
        }
    }
    public void Wait()
    {
        _turnActionComplete?.TrySetResult(true);
    }

    public void Move(int[] direction)
    {
        var wantedPosition = new []{Position[0] + direction[0], Position[1] + direction[1]};
        var wantedChunkPosition = GetChunkPosition(wantedPosition);
        var chunkPosition = GetChunkPosition(Position);

        var silly = ToLocalPosition(wantedPosition);
        System.Console.WriteLine("Local Chunk Position: " + silly[1] + ", " + silly[0]);


        if (GetTile(wantedPosition, Layer).Blocking) return;
        if (chunkPosition[0] != wantedChunkPosition[0] || chunkPosition[1] != wantedChunkPosition[1])
        {
            LoadSurroundingChunks(wantedChunkPosition[0], wantedChunkPosition[1], Layer);
        }

        Position[0] = wantedPosition[0];
        Position[1] = wantedPosition[1];

        _turnActionComplete?.TrySetResult(true);
    }
}