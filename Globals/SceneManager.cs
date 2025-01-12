using System.Threading.Tasks;
using Godot;
using Types;

namespace Globals
{
    public partial class SceneManager : Node
    {
        private string CurrentloadingScenePath;

        private LoadingScene CurrentloadingScene;

        private Node CurrentMainScene = null;

        public static SceneManager Instance { get; private set; }

        public void Initialize(string loadingScenePath = "res://UI/LoadingScenes/BlackedLoadingScene/blackLoadingScene.tscn")
        {
            this.CurrentloadingScenePath = loadingScenePath;
        }

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            GD.Print("SceneManager Initialized");

            // Remove the Game node if it exists and load the startingScene. This Game node is used as a placeholder to create empty 2D scene in the game.
            var _GameNode = GetNode("/root/Game");
            _GameNode?.QueueFree();
        }

        /// <summary>
        /// Load the main scene to root and remove the currently active mainNode
        /// </summary>
        /// <param name="scenePath"></param>
        /// <param name="playAnimation"></param>
        /// <param name="inAnimationName"></param>
        /// <param name="outAnimationName"></param>
        /// <returns></returns>
        public async Task<T> LoadMainScene<T>(string scenePath, bool playAnimation = true, string inAnimationName = "fade_in", string outAnimationName = "fade_out") where T : Node
        {
            var newLoadedScene = await LoadScene(scenePath: scenePath, superNode: GetTree().Root, this.CurrentMainScene, playAnimation: playAnimation, inAnimationName: inAnimationName, outAnimationName: outAnimationName);
            if (newLoadedScene != null)
            {
                this.CurrentMainScene = newLoadedScene;
            }
            return (T)newLoadedScene;
        }

        /// <summary>
        /// Load a regular node to the specified root node
        /// </summary>
        /// <param name="scenePath"></param>
        /// <param name="superNode"></param>
        /// <param name="RemoveNode"></param>
        /// <returns></returns>
        public async Task<T> LoadSceneToNode<T>(string scenePath, Node superNode, Node RemoveNode = null) where T : Node
        {
            if (superNode == null) return null;
            return (T)await LoadScene(scenePath: scenePath, superNode: superNode, RemoveNode: RemoveNode, playAnimation: false);
        }

        private async Task<Node> LoadScene(string scenePath, Node superNode = null, Node RemoveNode = null, bool playAnimation = true, string inAnimationName = "fade_in", string outAnimationName = "fade_out")
        {
            // Disable Global Inputs
            InputManager.Instance.InputEnable = false;

            if (playAnimation)
            {
                // Load the loading Scene if it is not loaded already
                await LoadTheLoadingScene();

                // Make the Loading Scene Visible
                this.CurrentloadingScene.Visible = true;

                // If added then play the animation
                await CurrentloadingScene.StartAnimation(inAnimationName);
            }

            // clear removing node if it is not null
            RemoveNode?.QueueFree();

            // Run code asyncronously to load the new scene
            Node LoadedNewScene = await LoadSceneToTreeAsync(scenePath, superNode);

            if (LoadedNewScene == null)
            {
                GD.PrintErr($"Failed to load the scene: ${scenePath}");
                return null;
            }

            // Disable the new scene's processing and animations temporarily
            PauseScene(LoadedNewScene);

            // If the loadedScene is added to the tree then run the next animation
            if (playAnimation)
            {
                await CurrentloadingScene.StopAnimation(outAnimationName);

                // Make the Loading Screen Invisible
                this.CurrentloadingScene.Visible = false;
            }

            // Enable Global Inputs
            InputManager.Instance.InputEnable = true;

            // Unpause the new scene
            UnPauseScene(LoadedNewScene);

            return LoadedNewScene;
        }

        private static async Task<PackedScene> LoadPackedSceneAsync(string scenePath)
        {
            return await Task.Run(() => GD.Load<PackedScene>(scenePath));
        }

        private async Task<Node> LoadSceneToTreeAsync(string scenePath, Node rootNode)
        {
            var packedScene = await LoadPackedSceneAsync(scenePath);

            Node scene = packedScene.Instantiate();
            rootNode.CallDeferred("add_child", scene);

            // Await until the node is entered to the tree
            await ToSignal(scene, "tree_entered");

            // Await until the node's _Ready method called. await until the scene is fully initialized
            await ToSignal(scene, "ready");

            return scene;
        }

        private async Task RemoveSceneFromTree(Node node)
        {
            node.CallDeferred("queue_free");
            await ToSignal(node, "tree_exited");
        }

        private async Task LoadTheLoadingScene(string newLoadingScenePath = null)
        {
            if (newLoadingScenePath != null && newLoadingScenePath != CurrentloadingScenePath)
            {
                LoadingScene loadingScene = (LoadingScene)await LoadSceneToTreeAsync(newLoadingScenePath, GetTree().Root);
                if (loadingScene != null)
                {
                    this.CurrentloadingScene = loadingScene;
                    this.CurrentloadingScenePath = newLoadingScenePath;
                    return;
                }
            }
            if (CurrentloadingScene == null)
            {
                CurrentloadingScene = (LoadingScene)await LoadSceneToTreeAsync(this.CurrentloadingScenePath, GetTree().Root);
            }
        }

        private static void PauseScene(Node node)
        {
            node.ProcessMode = ProcessModeEnum.Disabled;
        }
        private static void UnPauseScene(Node node)
        {
            node.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}