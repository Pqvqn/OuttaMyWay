using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Game.state == GameState.Started)
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                Game.finish = Time.time;
                Game.state = GameState.Intermission;
            }
        }
    }
}
