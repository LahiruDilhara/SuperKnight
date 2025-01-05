using System.Threading.Tasks;
using Godot;

namespace Globals
{
    public partial class SceneManager : Node
    {
        // Path to the scene which loaded firstly when the game started
        private string StartingScene = "res://UI/MainUi/main_ui.tscn";

        // Preload the loading screne
        private PackedScene LoadingScrene = GD.Load<PackedScene>("res://UI/LoadingScene/loading_scene.tscn");

        public static SceneManager Instance { get; private set; }

        private Node CurrentScene = null;

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

            // Load the default scene
            LoadScene(StartingScene);
        }

        public async void LoadScene(string scenePath, Node superNode = null, string inAnimationName = "fade_in", string outAnimationName = "fade_out")
        {
            // Disable Global Inputs
            InputManager.Instance.InputEnable = false;

            LoadingScene loadingScene = (LoadingScene)LoadingScrene.Instantiate();

            // Show the loading screen while loading the resources
            GetTree().Root.CallDeferred("add_child", loadingScene);

            // await until the loading scene is added to the tree
            await ToSignal(loadingScene, "ready");

            // If added then play the animation
            await loadingScene.PlayAnimation(inAnimationName);

            // If supply super node then set it
            superNode ??= GetTree().Root;

            // clear current scene if there is any scene loaded
            CurrentScene?.QueueFree();

            // Run code asyncronously to load the new scene
            var loadedScene = await Task.Run(() => GD.Load<PackedScene>(scenePath));
            if (loadedScene == null)
            {
                GD.PrintErr($"Failed to load the scene: ${loadedScene}");
                return;
            }

            // Instanciate the new Scene
            CurrentScene = loadedScene.Instantiate();
            superNode.CallDeferred("add_child", CurrentScene);

            // Disable the new scene's processing and animations temporarily
            PauseScene(CurrentScene);

            // Wait until the new scene is successfully loaded and added to the tree
            await ToSignal(CurrentScene, "ready");

            // If the loadedScene is added to the tree then run the next animation
            await loadingScene.PlayAnimation(outAnimationName);

            // Remove the loading scene after the new scene successfully loaded
            GetTree().Root.CallDeferred("remove_child", loadingScene);

            // Enable Global Inputs
            InputManager.Instance.InputEnable = true;

            // Unpause the new scene
            UnPauseScene(CurrentScene);
        }

        private static void PauseScene(Node node)
        {
            node.SetProcess(false);
            node.SetProcessInput(false);
            node.SetPhysicsProcess(false);
        }
        private static void UnPauseScene(Node node)
        {
            node.SetProcess(true);
            node.SetProcessInput(true);
            node.SetPhysicsProcess(true);
        }
    }
}