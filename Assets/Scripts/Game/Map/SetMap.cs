using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMap : MonoBehaviour
{
    public Transform target;

    private void Awake()
    {
        FindObjectOfType<PlayerController>().transform.position = target.position;
    }
}
