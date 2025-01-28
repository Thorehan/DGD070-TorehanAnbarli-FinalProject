using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _players;
    private readonly GameContext _context;
    private readonly IGroup<GameEntity> _gameStates;

    public MovementSystem(Contexts contexts)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position, GameMatcher.MovementSpeed));
        _gameStates = _context.GetGroup(GameMatcher.GameState);
    }

    public void Execute()
    {
        var gameState = _gameStates.GetSingleEntity();
        if (gameState != null && gameState.gameState.isGameWon)
        {
            return;
        }

        foreach (var entity in _players)
        {
            var position = entity.position.value;
            var speed = entity.movementSpeed.value;
            var boundary = entity.boundary;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * speed * Time.deltaTime;
            Vector3 newPosition = position + movement;

            newPosition.x = Mathf.Clamp(newPosition.x, boundary.minX, boundary.maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, boundary.minZ, boundary.maxZ);

            entity.ReplacePosition(newPosition);
        }
    }
}