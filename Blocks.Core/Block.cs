namespace Blocks.Core
{
    public abstract class Block: BlockMap
    {
        public abstract void Transform(string name);
        internal abstract void OnStarted();
        internal abstract void OnStopped();
    }
}
