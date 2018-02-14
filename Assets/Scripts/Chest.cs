using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, OpenClose {

    public bool isOpen;
    private Animation myAnim;

    private void Awake()
    {
        myAnim = GetComponentInParent<Animation>();
    }

    // Use this for initialization
    void Start()
    {
        isOpen = false;
    }

    public void OpenClose(Vector3 direction)
    {
        if(!myAnim.isPlaying)
        {
            if (!isOpen)
                myAnim.Play("open");
            else
                myAnim.Play("close");


            isOpen = !isOpen;
        }
    }
}
