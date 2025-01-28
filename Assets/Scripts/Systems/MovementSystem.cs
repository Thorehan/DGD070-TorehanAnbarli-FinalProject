using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem
{
    IGroup<GameEntity> players;
    GameContext context;
    IGroup<GameEntity> gameStates;

    public MovementSystem(Contexts contexts)
    {
        context = contexts.game;
        players = context.GetGroup(GameMatcher.AllOf(
            GameMatcher.Player,
            GameMatcher.Position,
            GameMatcher.MovementSpeed));
        gameStates = context.GetGroup(GameMatcher.GameState);
    }

    public void Execute()
    {
        var gameState = gameStates.GetSingleEntity();
        if (gameState != null && gameState.gameState.isGameWon)
        {
            return;
        }

        foreach (var e in players)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 hareket = new Vector3(h, 0, v);
            hareket = hareket.normalized * e.movementSpeed.value * Time.deltaTime;

            Vector3 yeniPozisyon = e.position.value + hareket;

            if (e.hasBoundary)
            {
                yeniPozisyon.x = Mathf.Clamp(yeniPozisyon.x, e.boundary.minX, e.boundary.maxX);
                yeniPozisyon.z = Mathf.Clamp(yeniPozisyon.z, e.boundary.minZ, e.boundary.maxZ);
            }

            e.ReplacePosition(yeniPozisyon);
        }
    }
}