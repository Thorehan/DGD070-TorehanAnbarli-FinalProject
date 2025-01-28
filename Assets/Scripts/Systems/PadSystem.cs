using Entitas;
using UnityEngine;

public class PadSystem : IExecuteSystem
{
    GameContext context;
    IGroup<GameEntity> players;
    IGroup<GameEntity> pads;
    IGroup<GameEntity> gameStates;

    public PadSystem(Contexts contexts)
    {
        context = contexts.game;
        players = context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position));
        pads = context.GetGroup(GameMatcher.AllOf(GameMatcher.Pad, GameMatcher.Position));
        gameStates = context.GetGroup(GameMatcher.GameState);
    }

    public void Execute()
    {
        var player = players.GetSingleEntity();
        if (player == null) return;

        var gameState = gameStates.GetSingleEntity();
        if (gameState == null) return;

        foreach (var pad in pads)
        {
            float mesafe = Vector3.Distance(player.position.value, pad.position.value);

            if (!pad.pad.isActivated && mesafe < 1f)
            {
                pad.pad.isActivated = true;
                pad.view.gameObject.GetComponent<Renderer>().material.color = Color.green;

                gameState.ReplaceGameState(
                    gameState.gameState.isGameWon,
                    gameState.gameState.activatedPadsCount + 1
                );

                if (gameState.gameState.activatedPadsCount >= 4)
                {
                    gameState.gameState.isGameWon = true;

                    var winText = context.GetGroup(GameMatcher.WinText).GetSingleEntity();
                    if (winText != null && winText.winText.textMesh != null)
                    {
                        winText.winText.textMesh.gameObject.SetActive(true);
                        winText.winText.textMesh.text = "A WINRAR IS YOU";
                    }
                }
            }
        }
    }
}