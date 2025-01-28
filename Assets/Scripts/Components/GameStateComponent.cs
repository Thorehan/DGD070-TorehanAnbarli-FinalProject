using Entitas;
using UnityEngine;

[Game]
public sealed class GameStateComponent : IComponent
{
    public bool isGameWon;
    public int activatedPadsCount;
}