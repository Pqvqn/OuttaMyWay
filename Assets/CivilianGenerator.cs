using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianGenerator : MonoBehaviour
{
    public GameObject civilianPrefab;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject go = GameObject.Instantiate(civilianPrefab);
            go.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), -1);
        }
    }
}
