using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    void Start()
    {
        int TOTAL_ROOMS = 10;
        int TARGET_CIVILIANS = 0;
        List<SquareRoom> rooms = new List<SquareRoom>();
        SquareRoom lastRoom = null;
        int x = 0, y = 0;
        float totalArea = 0;
        int vertSize = 4 * TOTAL_ROOMS + 1;
        Vector2[] vertices = new Vector2[vertSize];
        for (int i = 0; i < TOTAL_ROOMS; i++)
        {
            int width = Random.Range(8, 15), height = Random.Range(6, 12), dy = Random.Range(-height/2, height/2);

            SquareRoom room = new SquareRoom(x, x + width, y + dy + height / 2.0f, y + dy - height / 2.0f);
            totalArea += room.area;
            if (lastRoom != null)
            {
                room.Prev = lastRoom;
                lastRoom.Next = room;
                if (lastRoom.top < room.top)
                {
                    vertices[2 * i] = new Vector2(room.left - 1, lastRoom.top + 1);
                    vertices[2 * i + 1] = new Vector2(room.left - 1, room.top + 1);
                    vertices[vertSize - 2 * i - 1] = new Vector2(lastRoom.right + 1, room.bottom - 1);
                    vertices[vertSize - 2 * i] = new Vector2(lastRoom.right + 1, lastRoom.bottom - 1);
                } else
                {
                    vertices[2 * i] = new Vector2(lastRoom.right + 1, lastRoom.top + 1);
                    vertices[2 * i + 1] = new Vector2(lastRoom.right + 1, room.top + 1);
                    vertices[vertSize - 2 * i - 1] = new Vector2(room.left - 1, room.bottom - 1);
                    vertices[vertSize - 2 * i] = new Vector2(room.left - 1, lastRoom.bottom - 1);
                }
            } else
            {
                vertices[0] = new Vector2(room.left - 1, room.bottom - 1);
                vertices[vertSize-1] = new Vector2(room.left - 1, room.bottom - 1);
                vertices[1] = new Vector2(room.left - 1, room.top + 1);
            }
            if (i == TOTAL_ROOMS - 1)
            {
                vertices[vertSize - 2 * i - 3] = new Vector2(room.right + 1, room.top + 1);
                vertices[vertSize - 2 * i - 2] = new Vector2(room.right + 1, room.bottom - 1);
            }
            lastRoom = room;
            rooms.Add(room);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.position = new Vector3(x + width / 2.0f, y + dy, 0);
            go.transform.localScale = new Vector3(width + 2, height + 2, 1);
            x += width - 1;
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
