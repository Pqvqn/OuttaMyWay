using UnityEngine;

public class ShoveInstance : MonoBehaviour
{
    public static GameObject Prefab { get
        {
            if (prefab == null)
            {
                prefab = Resources.Load("ActionPrefabs/ShovePrefab") as GameObject;
            }
            return prefab;
        }
    }
    private static GameObject prefab = null;
    private Quaternion dir;
    private float speed = 1f;
    private float timeLeft = 0f;
    private GenericShove shove;
    public void Initialize(GenericShove shove, Vector2 pos, Vector2 dir, float speed, float lifetime)
    {
        Player.instance.HideHands();
        transform.position = pos;
        transform.parent = Player.instance.transform;
        this.shove = shove;
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
        Collider2D[] overlap = Physics2D.OverlapBoxAll(transform.position + transform.forward * 0.25f, new Vector2(0.5f, 1f), dir.eulerAngles.x, 1 << 7);
        foreach (Collider2D col in overlap)
        {
            IInteractable target = col.gameObject.GetComponent<IInteractable>();
            if (target != null)
            {
                shove.Hit(target);
            }
        }
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            shove.EndShove();
        }
    }

    private void OnDestroy()
    {
        Player.instance.ShowHands();
    }
}
