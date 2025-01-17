using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject playerEffect;
    
    [Header("Animation Player")]
    public Animator anim;
    public AnimState animState;
    
    [Header("Moving")] 
    public float moveSpeed;
    private Vector2 nextMove;
    public Vector2 directionMove;
    public bool isMove;

    [Header("Connected Stone")] 
    public bool isConnected;
    public List<GameObject> conectedStones = new List<GameObject>();

    [Header("Check Ground")] 
    public bool isGrounded;
    public LayerMask groundLayer;

    [Header("Check wall")]
    public LayerMask wallLayer;
    public float distanceToWall;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //animation state
        ChangeAnimation();

        //check player on ground
        CheckGround();
        
        //set direction move and nextPoint player
        SetTargetMove();
        
        //check wall foward player or conectedStones forward
        RaycastCheckMove();
    }

    private void FixedUpdate()
    {
        //move player when not wall forward
        Moving();
    }

    void SetTargetMove()
    {
        if (directionMove != Vector2.zero && !isMove)
        {
            nextMove = transform.position + new Vector3(directionMove.x,directionMove.y);
            //isMove = true;

            if (directionMove.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (directionMove.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    void RaycastCheckMove()
    {
        if (!isMove && directionMove != Vector2.zero)
        {
            if (isConnected)
            {
                //check list obj conectedStones ray
                bool _iswall = false;
                foreach (GameObject conectedStone in conectedStones)
                {
                    RaycastHit2D hitStone = Physics2D.Raycast(conectedStone.transform.position, directionMove,
                        distanceToWall, wallLayer);
    
                    if (hitStone.collider != null)
                    {
                        _iswall = true;
                    }
                }
    
                //check ray player
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionMove, distanceToWall, wallLayer);
                
                //set 
                if (hit.collider == null && !_iswall)
                {
                    isMove = true;
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionMove, distanceToWall, wallLayer);
                
                if (hit.collider == null)
                {
                    isMove = true;
                }
            }
        }
    }

    void Moving()
    {
        if (isMove)
        {
            //moving
            rb.velocity = directionMove * moveSpeed;

            //check target move
            if (Vector3.Distance(transform.position, nextMove) < .1f)
            {
                transform.position = nextMove;
                isMove = false;
                
                directionMove = rb.velocity = Vector2.zero;
            }
        }
    }

    void ChangeAnimation()
    {
        anim.SetInteger("state", (int)animState);

        if (isConnected)
        {
            animState = AnimState.magnet;
        }
        else
        {
            if (directionMove == Vector2.zero)
            {
                animState = AnimState.idle;
            }
            else
            {
                animState = AnimState.run;
            }
        }
    }

    void CheckGround()
    {
        if (GameManager.Instance.isPlaying && !isMove)
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, .1f, groundLayer);
            
            
            if (isConnected)
            {
                if (!isGrounded && !CheckConnectedToGround())
                {
                    GameManager.Instance.UpdateGameState(GameState.GameEnd,false);
                    
                    ShakeCam.Instance.Shake();

                    GameObject fx = Instantiate(playerEffect, transform.position, Quaternion.identity);
                    Destroy(fx,1.5f);
                }
            }
            else
            {
                if (!isGrounded)
                {
                    GameManager.Instance.UpdateGameState(GameState.GameEnd,false);
                    
                    ShakeCam.Instance.Shake();
                    
                    GameObject fx = Instantiate(playerEffect, transform.position, Quaternion.identity);
                    Destroy(fx,1.5f);
                }
            }
        }
    }
    
    bool CheckConnectedToGround()
    {
        foreach (var connect in conectedStones)
        {
            if (connect.GetComponent<MagnetStone>() != null && connect.GetComponent<MagnetStone>().isGrounded)
            {
                return true;
            }
            
            if (connect.GetComponent<MagnetPsychic>() != null && connect.GetComponent<MagnetPsychic>().isGrounded)
            {
                return true;
            }
        }

        return false;
    }

    public void AddConectedStone(GameObject stone)
    {
        conectedStones.Add(stone);
        isConnected = true;
    }

    public void RemoveConectedStone(GameObject stone)
    {
        conectedStones.Remove(stone);

        if (conectedStones.Count == 0)
        {
            isConnected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("end"))
        {
            GameManager.Instance.UpdateGameState(GameState.GameEnd,true);
        }
    }
}

public enum AnimState
{
    idle,
    run,
    magnet
}
