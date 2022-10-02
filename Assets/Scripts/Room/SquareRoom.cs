using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRoom : IRoom
{
    public float left {get; private set; }
    public float right { get; private set; }
    public float top { get; private set; }
    public float bottom { get; private set; }
    public float area { get { return (right - left) * (top - bottom);} }
    public Vector2 center { get { return new Vector2((left + right) * 0.5f, (bottom + top) * 0.5f); } }
    public IRoom Prev { get; set; }
    public IRoom Next { get; set; }
    public SquareRoom(float left, float right, float top, float bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }
    public Vector2 RandomWanderPosition(Vector2 currentPos)
    {
        //return new Vector2(Random.Range(left, right), Random.Range(top, bottom));
        float horizontal = Random.Range(-1f,1f), vertical = Random.Range(-1f,1f);
        horizontal = (horizontal * horizontal * horizontal + 1) % 1;
        vertical = (vertical * vertical * vertical + 1) % 1;
        return new Vector2(Mathf.Lerp(left,right,horizontal), Mathf.Lerp(bottom, top, vertical));
    }

    public Vector2 RandomSpawnPosition()
    {
        return new Vector2(Random.Range(left, right), Random.Range(top, bottom));
    }

    public Vector2 ConnectionPosition(IRoom adjacentRoom, Vector2 currentPos)
    {
        SquareRoom room = adjacentRoom as SquareRoom;
        if (room == null)
        {
            throw new System.Exception("No ConnectPosition implementation for SquareRoom and type "+adjacentRoom.GetType().Name);
        }
        float leftBound, rightBound;
        if (room.left < left)
        {
            leftBound = left;
            rightBound = room.right;
        } else
        {
            leftBound = room.left;
            rightBound = right;
        }
        float topBound = Mathf.Min(top, room.top), bottomBound = Mathf.Max(bottom, room.bottom);
        return new Vector2(Random.Range(leftBound, rightBound), Random.Range(bottomBound, topBound));
    }

    public bool Within(Vector2 currentPos)
    {
        return currentPos.x >= left && currentPos.x <= right && currentPos.y >= bottom && currentPos.y <= top;
    }

    public bool NeedsClamp(Vector2 currentPos, out Vector2 newPos)
    {
        newPos = currentPos;
        if (!Within(currentPos))
        {
            newPos = new Vector2(Mathf.Clamp(currentPos.x, left, right), Mathf.Clamp(currentPos.y, bottom, top));
            return true;
        }
        return false;
    }
}
