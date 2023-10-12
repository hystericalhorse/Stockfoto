using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField] SubMenu titleUI;
    [SerializeField] SubMenu gameWinUI;
    [SerializeField] SubMenu pauseUI;
    [SerializeField] SubMenu controlsUI;

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

	private void Awake()
	{
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            gameManager = this;
        }

        DontDestroyOnLoad(gameObject);
	}

	private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        switch (state)
        { 
            case State.TITLE:
                titleUI.MakeActive();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //gameMusic.Stop();
                break;
            case State.START_GAME:
                Cursor.lockState = CursorLockMode.Locked;
                titleUI.MakeInactive();
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
                    gameWinUI.MakeActive();
                    state = State.TITLE;
                }
                break;
            case State.PAUSE_GAME:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                pauseUI.MakeActive();
                controlsUI.MakeInactive();
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

    [Serializable]
    public struct SubMenu
    {
        public GameObject parentObject;
        public GameObject defaultSelect;

        public void MakeActive()
        {
            if (parentObject is null) return;
            parentObject.SetActive(true);
            if (defaultSelect is null)
                EventSystem.current.SetSelectedGameObject(defaultSelect);
        }

        public void MakeInactive()
        {
			if (parentObject is null) return;
			parentObject.SetActive(false);
		}
    }
}
