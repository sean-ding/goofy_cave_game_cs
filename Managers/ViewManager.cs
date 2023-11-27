using CaveGame.Scenes;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Program;
using static CaveGame.GameSettings;

namespace CaveGame.Managers;

public static class ViewManager
{
    public static void UpdateView(Player player)
    {
        var glyphs = new ColoredGlyph[GAMEVIEW_WIDTH,GAMEVIEW_HEIGHT];
        var chunkPosition = GetChunkPosition(player.Position);
        var chunk = GetChunk(chunkPosition[0], chunkPosition[1], player.Layer);
        Chunk? chunkN = null;
        Chunk? chunkNE = null;
        Chunk? chunkE = null;
        Chunk? chunkSE = null;
        Chunk? chunkS = null;
        Chunk? chunkSW = null;
        Chunk? chunkW = null;
        Chunk? chunkNW = null;
        var yOffset = player.Position[0] - GAMEVIEW_HEIGHT / 2;
        var xOffset = player.Position[1] - GAMEVIEW_WIDTH / 2;
        for (var y = 0; y < GAMEVIEW_HEIGHT; y++)
        {
            for (var x = 0; x < GAMEVIEW_WIDTH; x++ )
            {
                var currentChunk = chunk;
                var offsetPosition = new[] { y + yOffset, x + xOffset };
                var chunkOffsetPosition = GetChunkPosition(offsetPosition);
                var chunkOffset = ToLocalPosition(offsetPosition);
                if (chunkOffsetPosition[0] == chunkPosition[0] - 1)
                {
                    // northwest
                    if (chunkOffsetPosition[1] == chunkPosition[1] - 1)
                    {
                        if (chunkNW == null) { chunkNW = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkNW;
                    }
                    // northeast
                    else if (chunkOffsetPosition[1] == chunkPosition[1] + 1)
                    {
                        if (chunkNE == null) { chunkNE = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkNE;
                    }
                    // north
                    else if (chunkOffsetPosition[1] == chunkPosition[1])
                    {
                        if (chunkN == null) { chunkN = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkN;
                    }
                }
                else if (chunkOffsetPosition[0] == chunkPosition[0] + 1)
                {
                    // southwest
                    if (chunkOffsetPosition[1] == chunkPosition[1] - 1)
                    {
                        if (chunkSW == null) { chunkSW = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkSW;
                    }
                    // southeast
                    else if (chunkOffsetPosition[1] == chunkPosition[1] + 1)
                    {
                        if (chunkSE == null) { chunkSE = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkSE;
                    }
                    // south
                    else if (chunkOffsetPosition[1] == chunkPosition[1])
                    {
                        if (chunkS == null) { chunkS = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                        currentChunk = chunkS;
                    }
                }
                // west
                else if (chunkOffsetPosition[1] == chunkPosition[1] - 1)
                {
                    if (chunkW == null) { chunkW = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                    currentChunk = chunkW;
                }
                // east
                else if (chunkOffsetPosition[1] == chunkPosition[1] + 1)
                {
                    if (chunkE == null) { chunkE = GetChunk(chunkOffsetPosition[0], chunkOffsetPosition[1], player.Layer); }
                    currentChunk = chunkE;
                }
                // center is default

                var entity = currentChunk.EntityManager.GetEntity(offsetPosition);
                if (entity != null)
                {
                    glyphs[x, y] = entity.Glyph;
                }
                else
                {
                    glyphs[x, y] = currentChunk.Tiles[chunkOffset[0],chunkOffset[1]].Glyph;
                }
            }
        }
        
        glyphs[GAMEVIEW_WIDTH / 2, GAMEVIEW_HEIGHT / 2] = player.Glyph;
        GetGameScreen().UpdateScreen(glyphs);
    }
}