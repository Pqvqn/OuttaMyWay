using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    void Start()
    {
        for (int i = 0; i < 500; i++)
        {
            GameObject go = GameObject.Instantiate(civilianPrefab);
            go.transform.position = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), -1);
        }
    }
}
