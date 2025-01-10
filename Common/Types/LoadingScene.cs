using System.Threading.Tasks;
using Godot;

namespace Types
{
    public abstract partial class LoadingScene : CanvasLayer
    {
        // this is called to play the starting animation
        public abstract Task StartAnimation(string animationName);

        // this is called to change and play a new animation
        public abstract Task ChangeAnimation(string animatioName);

        // this is used to play the stop animation
        public abstract Task StopAnimation(string animationName);

        // this is used to stop the animations immediatly
        public abstract Task StopAnimationsNow();

    }
}