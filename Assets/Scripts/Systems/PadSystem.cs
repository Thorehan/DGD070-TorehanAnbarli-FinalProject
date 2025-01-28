using Entitas;
using UnityEngine;

public class PadSystem : IExecuteSystem
{
    private readonly GameContext _context;
    private readonly IGroup<GameEntity> _players;
    private readonly IGroup<GameEntity> _pads;
    private readonly IGroup<GameEntity> _gameStates;

    public PadSystem(Contexts contexts)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position));
        _pads = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Pad, GameMatcher.Position));
        _gameStates = _context.GetGroup(GameMatcher.GameState);
    }

    public void Execute()
    {
        var player = _players.GetSingleEntity();
        if (player == null) return;

        var gameStateEntity = _gameStates.GetSingleEntity();
        if (gameStateEntity == null) return;

        foreach (var pad in _pads)
        {
            if (!pad.pad.isActivated && Vector3.Distance(player.position.value, pad.position.value) < 1f)
            {
                pad.pad.isActivated = true;
                pad.view.gameObject.GetComponent<Renderer>().material.color = Color.cyan;

                gameStateEntity.ReplaceGameState(
                    gameStateEntity.gameState.isGameWon,
                    gameStateEntity.gameState.activatedPadsCount + 1
                );

                if (gameStateEntity.gameState.activatedPadsCount >= 4)
                {
                    gameStateEntity.ReplaceGameState(true, gameStateEntity.gameState.activatedPadsCount);
                    Debug.Log("A WINRAR IS YOU");
                }
            }
        }
    }
}