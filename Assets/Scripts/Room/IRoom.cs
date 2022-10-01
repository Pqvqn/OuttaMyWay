using UnityEngine;

public interface IRoom
{
    public IRoom Prev { get; }
    public IRoom Next { get; }
    public Vector2 RandomSpawnPosition();
    public Vector2 RandomWanderPosition(Vector2 currentPos);
    public Vector2 ConnectionPosition(IRoom adjacentRoom, Vector2 currentPos);
    public bool Within(Vector2 currentPos);
    public bool NeedsClamp(Vector2 currentPos, out Vector2 newPos);
}