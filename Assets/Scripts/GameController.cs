using UnityEngine;
using Entitas;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winTextMesh;

    private Systems _systems;
    private Contexts _contexts;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _systems = CreateSystems(_contexts);

        InitializeGame();

        _systems.Initialize();
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Game")
            .Add(new MovementSystem(contexts))
            .Add(new PadSystem(contexts))
            .Add(new ViewSystem(contexts));
    }

    private void InitializeGame()
    {
        var player = _contexts.game.CreateEntity();
        player.isPlayer = true;
        player.AddPosition(Vector3.zero);
        player.AddMovementSpeed(5f);
        player.AddView(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        player.AddBoundary(-8f, 8f, -4f, 4f);

        CreatePad(new Vector3(-5f, 0f, 3f));
        CreatePad(new Vector3(5f, 0f, 3f));
        CreatePad(new Vector3(-5f, 0f, -3f));
        CreatePad(new Vector3(5f, 0f, -3f));

        var gameState = _contexts.game.CreateEntity();
        gameState.AddGameState(false, 0);

        var winTextEntity = _contexts.game.CreateEntity();
        winTextEntity.AddWinText(winTextMesh);
        winTextMesh.gameObject.SetActive(false);
    }

    private void CreatePad(Vector3 position)
    {
        var pad = _contexts.game.CreateEntity();
        pad.AddPad(false);
        pad.AddPosition(position);

        var padObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        padObject.transform.position = position;
        padObject.transform.localScale = new Vector3(1f, 0.1f, 1f);

        pad.AddView(padObject);
    }
}