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
        var chunk = GetChunk(chunkPosition.Y, chunkPosition.X, player.Layer);
        Chunk? chunkN = null;
        Chunk? chunkNE = null;
        Chunk? chunkE = null;
        Chunk? chunkSE = null;
        Chunk? chunkS = null;
        Chunk? chunkSW = null;
        Chunk? chunkW = null;
        Chunk? chunkNW = null;
        var yOffset = player.Position.Y - GAMEVIEW_HEIGHT / 2;
        var xOffset = player.Position.X - GAMEVIEW_WIDTH / 2;
        for (var y = 0; y < GAMEVIEW_HEIGHT; y++)
        {
            for (var x = 0; x < GAMEVIEW_WIDTH; x++)
            {
                var chunkOffset = ToLocalPosition(new Point(y + yOffset, x + xOffset));
                var chunkYOffset = chunkOffset.Y;
                var chunkXOffset = chunkOffset.X;
                if (y + yOffset < chunkPosition.Y * CHUNK_HEIGHT)
                {
                    // northwest
                    if (x + xOffset < chunkPosition.X * CHUNK_WIDTH)
                    {
                        if (chunkNW == null) { chunkNW = GetChunk(chunkPosition.Y - 1, chunkPosition.X - 1, player.Layer); }
                        glyphs[x, y] = chunkNW.Tiles[chunkYOffset, chunkXOffset].Glyph;
                    }
                    // northeast
                    else if (x + xOffset >= chunkPosition.X * CHUNK_WIDTH + CHUNK_WIDTH)
                    {
                        if (chunkNE == null) { chunkNE = GetChunk(chunkPosition.Y - 1, chunkPosition.X + 1, player.Layer); }
                        glyphs[x, y] = chunkNE.Tiles[chunkYOffset,chunkXOffset].Glyph;
                    }
                    // north
                    else
                    {
                        if (chunkN == null) { chunkN = GetChunk(chunkPosition.Y - 1, chunkPosition.X, player.Layer); }
                        glyphs[x, y] = chunkN.Tiles[chunkYOffset,chunkXOffset].Glyph;
                    }
                }
                else if (y + yOffset >= chunkPosition.Y * CHUNK_HEIGHT + CHUNK_HEIGHT)
                {
                    // southwest
                    if (x + xOffset < chunkPosition.X * CHUNK_WIDTH)
                    {
                        if (chunkSW == null) { chunkSW = GetChunk(chunkPosition.Y + 1, chunkPosition.X - 1, player.Layer); }
                        glyphs[x, y] = chunkSW.Tiles[chunkYOffset,chunkXOffset].Glyph;
                    }
                    // southeast
                    else if (x + xOffset >= chunkPosition.X * CHUNK_WIDTH + CHUNK_WIDTH)
                    {
                        if (chunkSE == null) { chunkSE = GetChunk(chunkPosition.Y + 1, chunkPosition.X + 1, player.Layer); }
                        glyphs[x, y] = chunkSE.Tiles[chunkYOffset,chunkXOffset].Glyph;
                    }
                    // south
                    else
                    {
                        if (chunkS == null) { chunkS = GetChunk(chunkPosition.Y + 1, chunkPosition.X, player.Layer); }
                        glyphs[x, y] = chunkS.Tiles[chunkYOffset,chunkXOffset].Glyph;
                    }
                }
                // west
                else if (x + xOffset < chunkPosition.X * CHUNK_WIDTH)
                {
                    if (chunkW == null) { chunkW = GetChunk(chunkPosition.Y, chunkPosition.X - 1, player.Layer); }
                    glyphs[x, y] = chunkW.Tiles[chunkYOffset,chunkXOffset].Glyph;
                }
                // east
                else if (x + xOffset >= chunkPosition.X * CHUNK_WIDTH + CHUNK_WIDTH)
                {
                    if (chunkE == null) { chunkE = GetChunk(chunkPosition.Y, chunkPosition.X + 1, player.Layer); }
                    glyphs[x, y] = chunkE.Tiles[chunkYOffset,chunkXOffset].Glyph;
                }
                // center
                else
                {
                    glyphs[x, y] = chunk.Tiles[chunkYOffset,chunkXOffset].Glyph;
                }
            }
        }
        
        GetGameScreen().UpdateScreen(glyphs);
    }
}