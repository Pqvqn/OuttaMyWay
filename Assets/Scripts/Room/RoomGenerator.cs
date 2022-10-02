using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    public GameObject tablePrefab;
    public GameObject columnPrefab;
    public GameObject doorPrefab;
    public static RoomGenerator instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    private GameObject levelParent;
    public void NextLevel() {
        Game.level += 1;
        int TOTAL_ROOMS = 9 + Game.level;
        int TARGET_OBSTACLES = TOTAL_ROOMS + Game.level * 4;
        int TARGET_CIVILIANS = TOTAL_ROOMS * (25 + Game.level);
        if (levelParent != null)
        {
            Destroy(levelParent);
        }
        levelParent = new GameObject("Level " + Game.level);
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
                } else
                {
                    vertices[2 * i] = new Vector2(lastRoom.right + 1, lastRoom.top + 1);
                    vertices[2 * i + 1] = new Vector2(lastRoom.right + 1, room.top + 1);
                }
                if (lastRoom.bottom < room.bottom)
                {
                    vertices[vertSize - 2 * i - 1] = new Vector2(lastRoom.right + 1, room.bottom - 1);
                    vertices[vertSize - 2 * i] = new Vector2(lastRoom.right + 1, lastRoom.bottom - 1);
                } else
                {
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
                GameObject door = Instantiate(doorPrefab, levelParent.transform);
                door.transform.position = new Vector3(room.right + 1, Random.Range(room.bottom + 1, room.top - 1), 0);
                vertices[vertSize - 2 * i - 3] = new Vector2(room.right + 1, room.top + 1);
                vertices[vertSize - 2 * i - 2] = new Vector2(room.right + 1, room.bottom - 1);
            }
            lastRoom = room;
            rooms.Add(room);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.parent = levelParent.transform;
            go.transform.position = new Vector3(x + width / 2.0f, y + dy, 0);
            go.transform.localScale = new Vector3(width + 2, height + 2, 1);
            x += width - 1;
            y += dy;
        }
        levelParent.AddComponent<EdgeCollider2D>().SetPoints(new List<Vector2>(vertices));
        levelParent.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (SquareRoom room in rooms)
        {
            int citizens = Mathf.FloorToInt(TARGET_CIVILIANS * room.area / totalArea);
            for (int i = 0; i < citizens; i++)
            {
                Vector2 spawnPos = room.RandomSpawnPosition();
                GameObject go = Instantiate(civilianPrefab, levelParent.transform);
                Civilian civilian = go.GetComponent<Civilian>();
                civilian.SetRoom(room);
                go.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0);
            }
            int obstacles = Mathf.FloorToInt(TARGET_OBSTACLES * room.area / totalArea);
            for (int i = 0; i < obstacles; i++)
            {
                GameObject go = Instantiate(Random.Range(0,1f)>0.5f?tablePrefab:columnPrefab, levelParent.transform);
                go.transform.position = new Vector3(room.left + (room.right - room.left) * (1 + i) / (obstacles+1), Random.Range(room.bottom + 2, room.top - 2), 0);
            }
        }
        Player.instance.transform.position = rooms[0].center;
    }
}
