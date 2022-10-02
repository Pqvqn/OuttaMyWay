using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject target;
    public Vector3 positionOffset;
    [SerializeField] float followSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position + positionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + positionOffset, followSpeed * Time.deltaTime);
    }
}
