using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    void Start()
    {
        int x = 0, y = 0;
        for (int i = 0; i < 10; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            int width = Random.Range(10, 30), height = Random.Range(3, 6) * Random.Range(3, 6), dy = Random.Range(-height/2, height/2);
            go.transform.position = new Vector3(x + width / 2 - 1, y + dy, 0);
            go.transform.localScale = new Vector3(width, height, 1);
            x += width - 3;
            y += dy;
        }
        
    }
}
