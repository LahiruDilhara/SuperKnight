using System.Threading.Tasks;
using Godot;

namespace Types
{
    public abstract partial class LoadingScene : CanvasLayer
    {
        public abstract Task PlayAnimation(string animationName);
    }
}