using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagnetPsychic : MonoBehaviour
{
    private AudioSource source;
    
    private Transform player;

    [Header("Sprite")] 
    public SpriteRenderer spr;
    public Sprite spriteConnect;

    [Header("Connect")]
    public bool isConnect;
    public Vector2 directionConnect;
    public Transform targetConect;

    [Header("Psychic Connect to Stone")]
    public GameObject[] stones;

    [Header("Check ground")] 
    public bool isGrounded;
    public LayerMask groundLayer;
    
    [Header("FX connect")]
    public Transform fxPoint;
    public GameObject fxConnect;
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
        //set connect magnet
        SetConnect(player);
        
        //magnet follow after connect
        FollowingAfterConnect();

        //check connect to stone
        PsychicConnectStone();

        //check magnet to ground. no connect and no ground => destroy
        CheckGround();
    }

    public void SetConnect(Transform _target)
    {
        if (Vector2.Distance(transform.position, _target.position) < 1.05f && !isConnect)
        {
            source.Play();
            isConnect = true;
            
            spr.sprite = spriteConnect;
            
            //add obj => list stone player connect
            player.GetComponent<PlayerController>().AddConectedStone(gameObject);
            
            //direction Connect to _target
            directionConnect = _target.position - transform.position;
            directionConnect = new Vector2(
                Mathf.Round(directionConnect.x),
                Mathf.Round(directionConnect.y)
            );

            targetConect = _target;
            
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
            stones = GameObject.FindGameObjectsWithTag("stone");

            if (stones.Length > 0)
            {
                foreach (var stone in stones)
                {
                    stone.GetComponent<MagnetStone>().SetConnect(transform);
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