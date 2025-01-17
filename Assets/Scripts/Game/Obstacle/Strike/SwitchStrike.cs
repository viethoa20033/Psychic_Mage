using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStrike : MonoBehaviour
{
    public bool isTurnOn;

    public Animator anim;
    private void Update()
    {
        anim.SetBool("isTurnOn", isTurnOn);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("stone") || other.gameObject.CompareTag("psychic"))
        {
            isTurnOn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("stone") || other.gameObject.CompareTag("psychic"))
        {
            isTurnOn = true;
        }
    }
}
