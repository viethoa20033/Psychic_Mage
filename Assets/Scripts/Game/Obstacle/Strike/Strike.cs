using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    private SpriteRenderer spr;
    private BoxCollider2D coll;
    
    public Sprite[] sprites;

    public SwitchStrike switchStrike;

    public bool isDamage;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (switchStrike.isTurnOn)
        {
            spr.sprite = sprites[1];
            isDamage = true;
        }
        else
        {
            spr.sprite = sprites[0];
            isDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isDamage)
        {
            GameManager.Instance.UpdateGameState(GameState.GameEnd,false);
        }
    }
}
