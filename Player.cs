using CaveGame.Entities;

namespace CaveGame;

public class Player
{
    private Entity _entity;
    private readonly InputHandler _inputHandler;

    public string Name => _entity.Name;
    public int[] Position => _entity.Position;
    public int Layer => _entity.Layer;
    public ColoredGlyph Glyph => _entity.Glyph;
    
    public Player(Entity entity, InputHandler inputHandler)
    {
        _entity = entity;
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