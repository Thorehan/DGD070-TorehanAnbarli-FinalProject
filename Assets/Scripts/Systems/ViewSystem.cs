using Entitas;
using UnityEngine;

public class ViewSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _entities;

    public ViewSystem(Contexts contexts)
    {
        _entities = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.View, GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var entity in _entities)
        {
            entity.view.gameObject.transform.position = entity.position.value;
        }
    }
} 