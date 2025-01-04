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

        public async void LoadScene(string scenePath, Node superNode = null)
        {
            var loadingScene = LoadingScrene.Instantiate();

            // Show the loading screen while loading the resources
            GetTree().Root.CallDeferred("add_child", loadingScene);

            // If supply super node then set it
            superNode ??= GetTree().Root;

            // clear current scene if there is any scene loaded
            CurrentScene?.QueueFree();

            // var loadedScene = GD.Load<PackedScene>(scenePath);
            var loadedScene = await Task.Run(() => GD.Load<PackedScene>(scenePath));
            if (loadedScene == null)
            {
                GD.PrintErr($"Failed to load the scene: ${loadedScene}");
                return;
            }

            // Instanciate the loaded Scene
            CurrentScene = loadedScene.Instantiate();
            superNode.CallDeferred("add_child", CurrentScene);
            GetTree().Root.CallDeferred("remove_child", loadingScene);
        }
    }
}