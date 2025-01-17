using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void ButtonPlaying()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void ButtonPause()
    {
        GameManager.Instance.UpdateGameState(GameState.Pause);
    }

    public void ButtonChooseLevel()
    {
        GameManager.Instance.UpdateGameState(GameState.ChooseLevel);
    }

    public void ButtonRetry()
    {
        GameManager.Instance.UpdateGameState(GameState.GameEnd,false);

    }
}
