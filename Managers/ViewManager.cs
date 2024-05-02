using Microsoft.Xna.Framework.Graphics;
using static CaveGame.Managers.ChunkManager;
using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Viewcast;

namespace CaveGame.Managers;

public static class ViewManager
{
    public static void UpdateView(int posY, int posX, int layer)
    {
        var glyphs = new ColoredGlyph[GAMEVIEW_WIDTH, GAMEVIEW_HEIGHT];
        var viewMask = GetShadowMask(posY, posX, layer);
        var chunkPosition = GetChunkPosition(posY, posX);
        var chunk = GetChunk(chunkPosition.Y, chunkPosition.X, layer);
        Chunk? chunkN = null;
        Chunk? chunkNE = null;
        Chunk? chunkE = null;
        Chunk? chunkSE = null;
        Chunk? chunkS = null;
        Chunk? chunkSW = null;
        Chunk? chunkW = null;
        Chunk? chunkNW = null;
        var yOffset = posY - GAMEVIEW_HEIGHT / 2;
        var xOffset = posX - GAMEVIEW_WIDTH / 2;
        for (var y = 0; y < GAMEVIEW_HEIGHT; y++)
        {
            for (var x = 0; x < GAMEVIEW_WIDTH; x++)
            {
                var currentChunk = chunk;
                var offsetPosition = new Point(x + xOffset, y + yOffset);
                var chunkOffsetPosition = GetChunkPosition(offsetPosition.Y, offsetPosition.X);
                var chunkOffset = ToLocalPosition(offsetPosition.Y, offsetPosition.X);
                if (chunkOffsetPosition.Y == chunkPosition.Y - 1)
                {
                    // northwest
                    if (chunkOffsetPosition.X == chunkPosition.X - 1)
                    {
                        if (chunkNW == null) { chunkNW = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkNW;
                    }
                    // northeast
                    else if (chunkOffsetPosition.X == chunkPosition.X + 1)
                    {
                        if (chunkNE == null) { chunkNE = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkNE;
                    }
                    // north
                    else if (chunkOffsetPosition.X == chunkPosition.X)
                    {
                        if (chunkN == null) { chunkN = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkN;
                    }
                }
                else if (chunkOffsetPosition.Y == chunkPosition.Y + 1)
                {
                    // southwest
                    if (chunkOffsetPosition.X == chunkPosition.X - 1)
                    {
                        if (chunkSW == null) { chunkSW = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkSW;
                    }
                    // southeast
                    else if (chunkOffsetPosition.X == chunkPosition.X + 1)
                    {
                        if (chunkSE == null) { chunkSE = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkSE;
                    }
                    // south
                    else if (chunkOffsetPosition.X == chunkPosition.X)
                    {
                        if (chunkS == null) { chunkS = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                        currentChunk = chunkS;
                    }
                }
                // west
                else if (chunkOffsetPosition.X == chunkPosition.X - 1)
                {
                    if (chunkW == null) { chunkW = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                    currentChunk = chunkW;
                }
                // east
                else if (chunkOffsetPosition.X == chunkPosition.X + 1)
                {
                    if (chunkE == null) { chunkE = GetChunk(chunkOffsetPosition.Y, chunkOffsetPosition.X, layer); }
                    currentChunk = chunkE;
                }
                // center is default

                if (!viewMask[y, x])
                {
                    glyphs[x, y] = currentChunk.Tiles[chunkOffset.Y,chunkOffset.X].Glyph;
                    glyphs[x, y].Foreground = glyphs[x, y].Foreground.SetAlpha(0);
                    continue;
                }
                
                var entity = currentChunk.EntityManager.GetEntity(offsetPosition.Y, offsetPosition.X);
                if (entity != null)
                {
                    glyphs[x, y] = entity.Glyph;
                }
                else
                {
                    glyphs[x, y] = currentChunk.Tiles[chunkOffset.Y,chunkOffset.X].Glyph;
                    glyphs[x, y].Foreground = glyphs[x, y].Foreground.FillAlpha();
                }
            }
        }

        glyphs[GAMEVIEW_WIDTH / 2, GAMEVIEW_HEIGHT / 2] = GetPlayer().Glyph;
        GetGameScreen().UpdateScreen(glyphs);
    }
}