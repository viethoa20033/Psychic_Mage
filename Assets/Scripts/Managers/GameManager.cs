using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBase<GameManager>
{
    public static UnityAction<GameState> OnGameStateChanged;
    public GameState gameState;

    public bool isPlaying, isWin;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public void UpdateGameState(GameState newState, bool? _isWin = null)
    {
        gameState = newState;

        if (_isWin.HasValue)
        {
            isWin = _isWin.Value;
        }
        switch (newState)
        {
            case GameState.ChooseLevel:
                isWin = false;
                isPlaying = false;
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Pause:
                break;
            case GameState.GameEnd:
                HandleGameEnd();
                break;
        }
        OnGameStateChanged?.Invoke(gameState);
    }


    void HandlePlaying()
    {
        if (isWin)
        {
            LevelManager.Instance.NextLevel();
            isWin = false;
        }

        isPlaying = true;
    }

    void HandleGameEnd()
    {
        if (isPlaying)
        {
            isPlaying = false;
            if (!isWin)
            {
                StopAllCoroutines();
                StartCoroutine(GameRetry());
            }
        }
    }


    IEnumerator GameRetry()
    {
        yield return new WaitForSeconds(3f);
        LevelManager.Instance.LoadLevel();
        UpdateGameState(GameState.Playing);
        
    }
}

public enum GameState
{
    ChooseLevel,
    Playing,
    Pause,
    GameEnd
}