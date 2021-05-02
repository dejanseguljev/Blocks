namespace Blocks.Core
{
    public interface IRenderer
    {
        void Render(int x, int y, int width, int height, BlockMap blockMap, BlockMap background = null);
    }
}
