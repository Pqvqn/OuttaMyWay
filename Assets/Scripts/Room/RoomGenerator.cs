using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    void Start()
    {
        int TOTAL_ROOMS = 10;
        int TARGET_CIVILIANS = 250;
        List<SquareRoom> rooms = new List<SquareRoom>();
        SquareRoom lastRoom = null;
        int x = 0, y = 0;
        float totalArea = 0;
        int vertSize = 4 * TOTAL_ROOMS;
        Vector2[] vertices = new Vector2[vertSize];
        for (int i = 0; i < TOTAL_ROOMS; i++)
        {
            int width = Random.Range(6, 15), height = Random.Range(6, 12), dy = Random.Range(-height/2, height/2);

            SquareRoom room = new SquareRoom(x, x + width, y + dy + height / 2.0f, y + dy - height / 2.0f);
            totalArea += room.area;
            if (lastRoom != null)
            {
                room.Prev = lastRoom;
                lastRoom.Next = room;
                if (lastRoom.top < room.top)
                {
                    vertices[2 * i] = new Vector2(room.left, lastRoom.top);
                    vertices[2 * i + 1] = new Vector2(room.left, room.top);
                    vertices[vertSize - 2 * i] = new Vector2(lastRoom.right, room.bottom);
                    vertices[vertSize - 2 * i - 1] = new Vector2(lastRoom.right, lastRoom.bottom);
                } else
                {
                    vertices[2 * i] = new Vector2(lastRoom.right, lastRoom.top);
                    vertices[2 * i + 1] = new Vector2(lastRoom.right, room.top);
                    vertices[vertSize - 2 * i] = new Vector2(room.left, room.bottom);
                    vertices[vertSize - 2 * i - 1] = new Vector2(room.left, lastRoom.bottom);
                }
            } else
            {
                vertices[0] = new Vector2(room.left, room.bottom);
                vertices[1] = new Vector2(room.left, room.top);
            }
            if (i == TOTAL_ROOMS - 1)
            {
                vertices[vertSize - 2 * i - 3] = new Vector2(room.right, room.top);
                vertices[vertSize - 2 * i - 2] = new Vector2(room.right, room.bottom);
            }
            lastRoom = room;
            rooms.Add(room);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.position = new Vector3(x + width / 2.0f, y + dy, 0);
            go.transform.localScale = new Vector3(width + 2, height + 2, 1);
            x += width - 4;
            y += dy;
        }
        GetComponent<EdgeCollider2D>().SetPoints(new List<Vector2>(vertices));
        foreach (SquareRoom room in rooms)
        {
            int citizens = Mathf.FloorToInt(TARGET_CIVILIANS * room.area / totalArea);
            for (int i = 0; i < citizens; i++)
            {
                Vector2 spawnPos = room.RandomSpawnPosition();
                GameObject go = Instantiate(civilianPrefab);
                Civilian civilian = go.GetComponent<Civilian>();
                civilian.SetRoom(room);
                go.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
            }
        }
    }
}
