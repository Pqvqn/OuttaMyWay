using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    void Start()
    {
        List<SquareRoom> rooms = new List<SquareRoom>();
        SquareRoom lastRoom = null;
        int x = 0, y = 0;
        float totalArea = 0;
        for (int i = 0; i < 10; i++)
        {
            int width = Random.Range(6, 15), height = Random.Range(6, 12), dy = Random.Range(-height/2, height/2);

            SquareRoom room = new SquareRoom(x, x + width, y + dy + height / 2.0f, y + dy - height / 2.0f);
            totalArea += room.area;
            if (lastRoom != null)
            {
                room.Prev = lastRoom;
                lastRoom.Next = room;
            }
            lastRoom = room;
            rooms.Add(room);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.position = new Vector3(x + width / 2.0f, y + dy, 0);
            go.transform.localScale = new Vector3(width + 2, height + 2, 1);
            x += width - 4;
            y += dy;
        }

        int TARGET_CIVILIANS = 250;
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
