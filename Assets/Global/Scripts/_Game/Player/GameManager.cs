using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameWinUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject controlsUI;
    //[SerializeField] AudioSource gameMusic;
    //[SerializeField] AudioSource playerJump;

    public enum State
    {
        TITLE,
        START_GAME,
        PLAY_GAME,
        GAME_WIN,
        PAUSE_GAME,
    }

    public State state = State.START_GAME;
    float stateTimer = 0;
    public float timer = 0;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        switch (state)
        { 
            case State.TITLE:
                titleUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //gameMusic.Stop();
                break;
            case State.START_GAME:
                Cursor.lockState = CursorLockMode.Locked;
                titleUI.SetActive(false);
                //gameMusic.Play();
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    SetPause();
                }
                break;
            case State.GAME_WIN:
                timer = 0;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    gameWinUI.SetActive(false);
                    state = State.TITLE;
                }
                break;
            case State.PAUSE_GAME:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                pauseUI.SetActive(true);
                controlsUI.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        state = State.START_GAME;
    }

    public void SetPause()
    {
        state = State.PAUSE_GAME;
    }

    public void ResumeGame()
    {
        state = State.PLAY_GAME;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
