using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _players;
    private readonly GameContext _context;

    public MovementSystem(Contexts contexts)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position, GameMatcher.MovementSpeed));
    }

    public void Execute()
    {
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