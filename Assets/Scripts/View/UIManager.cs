using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject lockClick;

    [Header("Choose Level")] 
    public GameObject chooseLevel;
    public CanvasGroup bg;
    public RectTransform head;
    public RectTransform levelButtonRec;
    public Button[] levelButtons;

    [Header("Game Play")] 
    public RectTransform gamePlay;
    public RectTransform gamePause;
    public RectTransform headPause;
    public RectTransform gameSuccess;
    public RectTransform headSuccess;

    [Header("Load Map")] public Canvas canvas;
    public GameObject loadMap;
    public RectTransform left;
    public RectTransform right;

    [Header("Text")] 
    public Text levelText;

    [Header("Button Animation Click")] 
    public Button[] buttons;

    [Header("Change Music")] public bool isMusic;
    public Image[] imageMusics;
    public Sprite[] spriteMusics;
    private void Start()
    {
        GameManager.OnGameStateChanged += UpdateGameState;
        LevelManager.OnLevelChanged += UpdateLevel;
        

        //Addlistener onclick Button
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[i].onClick.AddListener(() => ButtonLevelClick(index));
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ButtonAnimationlClick(index));
        }
        
        
        //Add Animation UI Start Game
        head.anchoredPosition = new Vector2(0, Screen.height);
        head.DOAnchorPos(new Vector2(0, -125), .5f).SetEase(Ease.OutBack);

        levelButtonRec.anchoredPosition = new Vector2(0, Screen.height);
        levelButtonRec.DOAnchorPos(new Vector2(0, -100), .5f).SetEase(Ease.OutBack);
        
        //Head pause and gamesuccess
        headPause.DORotate(new Vector3(5,5,5), 1).SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        headPause.DOScale(Vector3.one * 1.2f, 1).SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        
        headSuccess.DORotate(new Vector3(5,5,5), 1).SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        headSuccess.DOScale(Vector3.one * 1.2f, 1).SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        
        //animation button level
        StartCoroutine(LevelButtonAnimation());
        
        
        //Load music in start game
        isMusic = PlayerPrefs.GetInt("isMusic", 1) == 1;
        if (isMusic)
        {
            foreach (var iamge in imageMusics)
            {
                AudioListener.volume = 1;
                iamge.sprite = spriteMusics[1];
            }
        }
        else
        {
            foreach (var iamge in imageMusics)
            {
                AudioListener.volume = 0;
                iamge.sprite = spriteMusics[0];
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateGameState;
        LevelManager.OnLevelChanged -= UpdateLevel;
    }

    void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.ChooseLevel:
                HandleChooseLevel();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.GameEnd:
                HandleGameEnd();
                break;
        }
    }

    void HandleChooseLevel()
    {
        lockClick.SetActive(true);

        Time.timeScale = 1;

        if (gamePause.gameObject.activeInHierarchy)
        {
            gamePause.DOAnchorPos(new Vector2(0, Screen.height), .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                lockClick.SetActive(false);

                gamePause.gameObject.SetActive(false);
                gamePlay.gameObject.SetActive(false);

                chooseLevel.SetActive(true);

                bg.alpha = 0;
                bg.DOFade(1, .5f);

                head.anchoredPosition = new Vector2(0, Screen.height);
                head.DOAnchorPos(new Vector2(0, -125), .5f).SetEase(Ease.OutBack);

                levelButtonRec.anchoredPosition = new Vector2(0, Screen.height);
                levelButtonRec.DOAnchorPos(new Vector2(0, -100), .5f).SetEase(Ease.OutBack);

                StartCoroutine(LevelButtonAnimation());
            });
        }

        if (gameSuccess.gameObject.activeInHierarchy)
        {
            gameSuccess.DOAnchorPos(new Vector2(0, Screen.height), .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                lockClick.SetActive(false);

                gameSuccess.gameObject.SetActive(false);
                gamePlay.gameObject.SetActive(false);

                chooseLevel.SetActive(true);

                bg.alpha = 0;
                bg.DOFade(1, .5f);

                head.anchoredPosition = new Vector2(0, Screen.height);
                head.DOAnchorPos(new Vector2(0, -125), .5f).SetEase(Ease.OutBack);

                levelButtonRec.anchoredPosition = new Vector2(0, Screen.height);
                levelButtonRec.DOAnchorPos(new Vector2(0, -100), .5f).SetEase(Ease.OutBack);

                
                StartCoroutine(LevelButtonAnimation());
            });
        }

    }

    void HandlePlaying()
    {
        lockClick.SetActive(true);

        if (chooseLevel.activeInHierarchy)
        {
            bg.alpha = 1;
            bg.DOFade(0, .5f);

            head.DOAnchorPos(new Vector2(0, Screen.height), .5f).SetEase(Ease.InBack);
            levelButtonRec.DOAnchorPos(new Vector2(0, -Screen.height), .5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                lockClick.SetActive(false);

                chooseLevel.SetActive(false);

                gamePlay.gameObject.SetActive(true);
                gamePlay.anchoredPosition = new Vector2(0, Screen.height);
                gamePlay.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutBack);

            });
        }

        if (gamePause.gameObject.activeInHierarchy)
        {
            gamePause.DOAnchorPos(new Vector2(0, Screen.height), .5f)
                .SetEase(Ease.InBack).SetUpdate(true).OnComplete(()
                    =>
                {
                    lockClick.SetActive(false);

                    Time.timeScale = 1;
                    gamePause.gameObject.SetActive(false);
                });
        }
        

        if (loadMap.activeInHierarchy)
        {
            left.DOAnchorPos(new Vector2(-Screen.width, 0), .5f).SetEase(Ease.InQuad);
            right.DOAnchorPos(new Vector2(Screen.width, 0), .5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                loadMap.SetActive(false);
                lockClick.SetActive(false);
            });
        }
        
        if (gameSuccess.gameObject.activeInHierarchy)
        {
            gameSuccess.DOAnchorPos(new Vector2(0, Screen.height), .5f)
                .SetEase(Ease.InBack).SetUpdate(true).OnComplete(()
                    =>
                {
                    lockClick.SetActive(false);

                    Time.timeScale = 1;
                    gameSuccess.gameObject.SetActive(false);
                });
        }
    }

    void HandlePause()
    {
        lockClick.SetActive(true);

        Time.timeScale = 0;

        gamePause.gameObject.SetActive(true);
        gamePause.anchoredPosition = new Vector2(0, Screen.height);
        gamePause.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
        {
            lockClick.SetActive(false);
        });
    }

    void HandleGameEnd()
    {
        if (GameManager.Instance.isWin)
        {
            lockClick.SetActive(true);
            StartCoroutine(GameSuccessAnimation());
        }
        else
        {
            if (!lockClick.activeInHierarchy)
            {
                lockClick.SetActive(true);
                
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                loadMap.gameObject.SetActive(true);

                left.anchoredPosition = new Vector2(-Screen.width, 0);
                right.anchoredPosition = new Vector2(Screen.width, 0);

                left.DOAnchorPos(new Vector2(-canvasRect.rect.width / 2, 0), 1.5f).SetEase(Ease.OutBounce);
                right.DOAnchorPos(new Vector2(canvasRect.rect.width / 2, 0), 1.5f).SetEase(Ease.OutBounce);
            }
        }
    }

    void ButtonLevelClick(int index)
    {
        LevelManager.Instance.SetLevel(index + 1);
    }

    void ButtonAnimationlClick(int index)
    {
        buttons[index].transform.DOScale(Vector3.one * .8f, .1f)
            .SetEase(Ease.InOutQuad).SetUpdate(true).OnComplete(() =>
            {
                buttons[index].transform.DOScale(Vector3.one, .1f)
                    .SetEase(Ease.InOutQuad).SetUpdate(true);
            });
    }

    void UpdateLevel(int level)
    {
        levelText.text = "LEVEL " + level;
    }

    public void ChangeMusic()
    {
        isMusic = !isMusic;

        if (isMusic)
        {
            foreach (var iamge in imageMusics)
            {
                AudioListener.volume = 1;
                iamge.sprite = spriteMusics[1];
            }
        }
        else
        {
            foreach (var iamge in imageMusics)
            {
                AudioListener.volume = 0;
                iamge.sprite = spriteMusics[0];
            }
        }
        PlayerPrefs.SetInt("isMusic", isMusic ? 1 : 0);
        PlayerPrefs.Save();
    }

    IEnumerator GameSuccessAnimation()
    {
        yield return new WaitForSeconds(.5f);
        gameSuccess.gameObject.SetActive(true);
        gameSuccess.anchoredPosition = new Vector2(0, Screen.height);
        gameSuccess.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            lockClick.SetActive(false);
        });
    }
    IEnumerator LevelButtonAnimation()
    {
        foreach (var levelButton in levelButtons)
        {
            levelButton.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.075f);
        }
    }
}
