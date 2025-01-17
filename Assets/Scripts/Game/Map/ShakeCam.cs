using System;
using UnityEngine;
using DG.Tweening; 

public class ShakeCam : SingletonBase<ShakeCam>
{
    private Transform cameraTransform; 
    
    private Vector3 originalPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        originalPosition = cameraTransform.localPosition; 
    }

    public void Shake()
    {
        cameraTransform.DOShakePosition(.5f, .5f, 20)
            .OnComplete(() => cameraTransform.localPosition = originalPosition); 
    }
}