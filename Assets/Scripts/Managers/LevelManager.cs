using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBase<LevelManager>
{
    public static UnityAction<int> OnLevelChanged;
    public int level;

    public GameObject[] levelMaps;
    public Transform gridmap;

    public void SetLevel(int _level)
    {
        level = _level;
        OnLevelChanged?.Invoke(level);

        SpawnMap();
    }

    public void LoadLevel()
    {
        SetLevel(level);
    }

    public void NextLevel()
    {
        if (level < 15)
        {
            level++;
            OnLevelChanged?.Invoke(level);

            SpawnMap();
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    void SpawnMap()
    {
        foreach (Transform map in gridmap)
        {
            Destroy(map.gameObject);
        }

        Instantiate(levelMaps[level - 1], gridmap);
    }
}
