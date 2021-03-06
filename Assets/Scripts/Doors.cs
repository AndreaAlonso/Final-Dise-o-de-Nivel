﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, OpenClose {

    public bool isOpen;
    private Animation myAnim;
    private string lastClip;
    public bool inverseDoor;
    public bool needKey;

    private void Awake()
    {
        myAnim = GetComponentInParent<Animation>();
    }

    // Use this for initialization
    void Start ()
    {
        isOpen = false;
    }
	
    public void OpenClose(Vector3 direction)
    {
        if (!myAnim.isPlaying)
        {
            if (isOpen)
            {
                if (lastClip == "Open")
                    myAnim.clip = myAnim.GetClip("Close");
                else
                    myAnim.clip = myAnim.GetClip("CloseInverse");

                myAnim.Play();
                isOpen = false;
            }
            else
            {
                if ((direction.z > 0 && transform.eulerAngles.y == 180) ||
                    (direction.x > 0 && transform.eulerAngles.y == 90))
                {
                    if (!inverseDoor)
                    {
                        myAnim.clip = myAnim.GetClip("Open");
                        lastClip = "Open";
                    }
                    else
                    {
                        myAnim.clip = myAnim.GetClip("OpenInverse");
                        lastClip = "OpenInverse";
                    }
                }
                else
                {
                    if (!inverseDoor)
                    {
                        myAnim.clip = myAnim.GetClip("OpenInverse");
                        lastClip = "OpenInverse";
                    }
                    else
                    {
                        myAnim.clip = myAnim.GetClip("Open");
                        lastClip = "Open";
                    }
                }

                myAnim.Play();
                isOpen = true;
            }
        }
    }
}
