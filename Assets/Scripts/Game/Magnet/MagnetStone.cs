using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class MagnetStone : MonoBehaviour
{
    private AudioSource source;
    private Transform player;

    [Header("Sprite")] 
    public SpriteRenderer spr;
    public Sprite spriteConnect;

    [Header("Connect")]
    public bool isConnect;
    public Vector2 directionConnect;
    private Transform targetConect;
    
    [Header("Stone Connect to psychic")]
    public GameObject[] psychics;
    
    [Header("Check Ground")] 
    public bool isGrounded;
    public LayerMask groundLayer;

    [Header("FX connect")]
    public Transform fxPoint;
    public GameObject fxConnect;
    public GameObject effectDestroy;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //Set connect to player
        SetConnect(player);
        
        //stone following target after connect
        FollowingAfterConnect();

        //connect to psychic
        PsychicConnectStone();
        
        //stone check ground
        CheckGround();

    }
    
    public void SetConnect(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) < 1.05f && !isConnect)
        {
            source.Play();
            isConnect = true;
            
            player.GetComponent<PlayerController>().AddConectedStone(gameObject);

            if (spr != null)
            {
                spr.sprite = spriteConnect;
            }
            
            directionConnect = target.position - transform.position;
            directionConnect = new Vector2(
                Mathf.Round(directionConnect.x),
                Mathf.Round(directionConnect.y)
            );

            targetConect = target;

            
            //Spawn fx
            GameObject _fx = Instantiate(fxConnect, fxPoint);
            
            var directionTuple = (directionConnect.x, directionConnect.y);
            switch (directionTuple)
            {
                case (1, 0):
                    _fx.transform.rotation = Quaternion.Euler(0, 0, 0);
                    return;
                case (0, 1):
                    _fx.transform.rotation = Quaternion.Euler(0, 0, 90);
                    return;
                case (-1, 0):
                    _fx.transform.rotation = Quaternion.Euler(0, 0, -180);
                    return;
                case (0, -1):
                    _fx.transform.rotation = Quaternion.Euler(0, 0, -90);
                    return;
            }
            
        }
    }
    void FollowingAfterConnect()
    {
        if (isConnect)
        {
            Vector2 vectorDistance = targetConect.position - new Vector3(directionConnect.x, directionConnect.y);
            
            transform.position = vectorDistance;
        }
    }
    
    void PsychicConnectStone()
    {
        if (isConnect)
        {
            psychics = GameObject.FindGameObjectsWithTag("psychic");

            if (psychics.Length > 0)
            {
                foreach (var psychic in psychics)
                {
                    psychic.GetComponent<MagnetPsychic>().SetConnect(transform);
                }
            }
        }
    }
    
    void CheckGround()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down,.1f,groundLayer);

        if (!isConnect && !isGrounded)
        {
            StartCoroutine(DelayDestroy());
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        
        GameObject fx = Instantiate(effectDestroy, transform.position, Quaternion.identity);
        Destroy(fx,1.5f);
    }


    private void OnDestroy()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().RemoveConectedStone(this.gameObject);
        }
        
        ShakeCam.Instance.Shake();
    }
}
