using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInstance : MonoBehaviour
{
    public static GameObject Prefab
    {
        get
        {
            if (prefab == null)
            {
                prefab = Resources.Load("ActionPrefabs/GrabPrefab") as GameObject;
            }
            return prefab;
        }
    }
    private static GameObject prefab = null;
    private Quaternion dir;
    private float speed = 1f;
    private float timeLeft = 0f;
    private GenericGrab grab;
    public void Initialize(GenericGrab grab, Vector2 pos, Vector2 dir, float speed, float lifetime)
    {
        Player.instance.HideHands();
        transform.position = pos;
        transform.parent = Player.instance.transform;
        this.grab = grab;
        this.dir = Quaternion.LookRotation(dir, Vector3.forward);
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        this.speed = speed;
        this.timeLeft = lifetime;
    }
    public void FixedUpdate()
    {
        transform.position += dir * Vector3.forward * speed * Time.deltaTime;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Player.instance.ShowHands();
            grab.EndGrab();
            return;
        }
        Collider2D[] overlap = Physics2D.OverlapBoxAll(transform.position + transform.forward * 0.25f, new Vector2(0.5f, 1f), dir.x, 1 << 7);
        foreach (Collider2D col in overlap)
        {
            IInteractable target = col.gameObject.GetComponent<IInteractable>();
            if (target != null)
            {
                grab.Hit(target);
                grab.EndGrab();
            }
        }
        
    }
}
