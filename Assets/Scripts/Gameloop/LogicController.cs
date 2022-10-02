using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LogicController : MonoBehaviour
{
    public TMP_Text timer;
    public TMP_Text complete;
    public static LogicController instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    private GameState lastState = GameState.Menu;
    private void FixedUpdate()
    {
        if (lastState != Game.state)
        {
            // Once
            switch (Game.state)
            {
                case GameState.Starting:
                    RoomGenerator.instance.NextLevel();
                    //MusicController.instance.SetTime(0);
                    MusicController.instance.UnPause();
                    Game.deadline = Time.time + 40;
                    Game.state = GameState.Started;
                    break;
                case GameState.Started:
                    break;
                case GameState.Intermission:
                    timer.text = (Game.deadline - Game.finish).ToString("F1");
                    complete.text = "Level " + Game.level + " complete!";
                    MusicController.instance.currentTrack = 0;
                    break;
            }
            lastState = Game.state;
        }
        else
        {
            // Every tick
            switch (Game.state)
            {
                case GameState.Started:
                    complete.text = "";
                    MusicController.instance.currentTrack = Time.time + 20 > Game.deadline ? 1 : 0;
                    if (Time.time > Game.deadline)
                    {
                        Debug.Log("L");
                        Game.state = GameState.Menu;
                    }
                    break;
                case GameState.Intermission:
                    if (Game.finish + 5 < Time.time)
                    {
                        Game.state = GameState.Starting;
                    }
                    break;
            }
        }
    }
    private void Update()
    {
        switch (Game.state)
        {
            case GameState.Started:
                timer.text = (Game.deadline - Time.time).ToString("F1");
                break;
        }

    }
}
