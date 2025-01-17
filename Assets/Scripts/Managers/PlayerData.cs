using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public GameObject lockLevel;
    public Text leveltext;

    public Transform[] levelButtons;
    
    private void Start()
    {
        GameManager.OnGameStateChanged += UpdateGameState;
        
        
        LoadData();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
    }

    void UpdateGameState(GameState state)
    {
        if (state == GameState.GameEnd && GameManager.Instance.isWin)
        {
            SaveData(LevelManager.Instance.level);
        }
    }
    public void LoadData()
    {
        int levelMax = PlayerPrefs.GetInt("level", 0);

        foreach (Transform levelButton in levelButtons)
        {
            levelButton.GetComponent<Button>().interactable = false;
            foreach (Transform child in levelButton)
            {
                Destroy(child.gameObject);
            }
        }
        
        

        for (int i = 0; i < levelButtons.Length; i++)
        {
            
            if (i <= levelMax)
            {
                levelButtons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                Instantiate(lockLevel, levelButtons[i]);
            }
            Text txtLevel = Instantiate(leveltext, levelButtons[i]);
            txtLevel.text = (i + 1).ToString();
        }
    }

    public void SaveData(int level)
    {
        int levelMax = PlayerPrefs.GetInt("level", 0);

        if(level > levelMax)
        {
            Debug.Log("Save Level " + level);
            
            PlayerPrefs.SetInt("level", level);
            PlayerPrefs.Save();
            
            
            LoadData();
        }
    }
    
}
