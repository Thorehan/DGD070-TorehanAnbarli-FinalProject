using Entitas;
using UnityEngine;

[Game]
public sealed class BoundaryComponent : IComponent
{
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
}