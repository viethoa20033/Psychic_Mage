using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public GameObject aimLaser;
    public LayerMask targetLayer;

    [Header("Effect")]
    public ParticleSystem particle;
    public GameObject laserEffect;
    private void Start()
    {
        StartCoroutine(FireLaser());
    }

    void Damage()
    {
        particle.Play();
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up,Mathf.Infinity,targetLayer);

        foreach (var hit in hits)
        {
            if (hit.transform.GetComponent<PlayerController>())
            {
                GameManager.Instance.UpdateGameState(GameState.GameEnd,false);
                
                GameObject fx =  Instantiate(laserEffect, hit.transform.position, Quaternion.identity);
                Destroy(fx,1.5f);
                return;
                
            }
            if (hit.transform.gameObject != this.gameObject)
            {
                Destroy(hit.transform.gameObject);
               GameObject fx =  Instantiate(laserEffect, hit.transform.position, Quaternion.identity);
                Destroy(fx,1.5f);
            }
        }
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            aimLaser.SetActive(true);
            
            yield return new WaitForSeconds(1.5f);
            aimLaser.SetActive(false);
            
            Damage();
            
        }
    }
}
