using System.Threading.Tasks;
using Godot;
using Types;

namespace Globals
{
    public partial class SceneManager : Node
    {
        private string LoadingScenePath;

        private LoadingScene LoadingScene;

        public static SceneManager Instance { get; private set; }

        private Node CurrentScene = null;

        public void Initialize(string loadingScenePath = "res://UI/LoadingScenes/BlackedLoadingScene/blackLoadingScene.tscn")
        {
            this.LoadingScenePath = loadingScenePath;
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

            // Remove the Game node if it exists and load the startingScene. This Game node is used as place holder to create empty 2D scene in the game.
            var _GameNode = GetNode("/root/Game");
            _GameNode?.QueueFree();
        }

        public async void LoadScene(string scenePath, Node superNode = null, string inAnimationName = "fade_in", string outAnimationName = "fade_out")
        {
            await LoadTheLoadingScene();

            // Make the Loading Scene Visible
            this.LoadingScene.Visible = true;

            // Disable Global Inputs
            InputManager.Instance.InputEnable = false;

            // If added then play the animation
            await LoadingScene.PlayAnimation(inAnimationName);

            // If supply super node then set it
            superNode ??= GetTree().Root;

            // clear current scene if there is any scene loaded
            CurrentScene?.QueueFree();

            // Run code asyncronously to load the new scene
            Node LoadedNewScene = await LoadSceneToTreeAsync(scenePath, superNode);

            if (LoadedNewScene == null)
            {
                GD.PrintErr($"Failed to load the scene: ${scenePath}");
                return;
            }

            // Disable the new scene's processing and animations temporarily
            PauseScene(LoadedNewScene);

            // If the loadedScene is added to the tree then run the next animation
            await LoadingScene.PlayAnimation(outAnimationName);

            // Make the Loading Screen Invisible
            this.LoadingScene.Visible = false;

            // Enable Global Inputs
            InputManager.Instance.InputEnable = true;

            // Unpause the new scene
            UnPauseScene(LoadedNewScene);
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

        private async Task RemoveSceneFromTree(Node node, Node rootNode, bool awaitUntilRemoved)
        {
            rootNode.CallDeferred("remove_child", node);
        }

        private async Task LoadTheLoadingScene()
        {
            if (LoadingScene == null)
            {
                LoadingScene = (LoadingScene)await LoadSceneToTreeAsync(this.LoadingScenePath, GetTree().Root);
            }
        }

        private static void PauseScene(Node node)
        {
            node.SetProcess(false);
            node.SetProcessInput(false);
            node.SetPhysicsProcess(false);
            node.SetProcessInternal(false);
            node.SetPhysicsProcessInternal(false);

            foreach (Node subNode in node.GetChildren())
            {
                PauseScene(subNode);
            }
        }
        private static void UnPauseScene(Node node)
        {
            node.SetProcess(true);
            node.SetProcessInput(true);
            node.SetPhysicsProcess(true);
            node.SetProcessInternal(true);
            node.SetPhysicsProcessInternal(true);
            foreach (Node subNode in node.GetChildren())
            {
                UnPauseScene(subNode);
            }
        }
    }
}