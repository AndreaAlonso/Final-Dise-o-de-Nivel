using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

    public bool isOpen;
    private Animation myAnim;
    private string lastClip;

    private void Awake()
    {
        myAnim = GetComponentInParent<Animation>();
    }

    // Use this for initialization
    void Start () {
        isOpen = false;
    }
	
	public void OpenOrClose(Vector3 doorDirection)
    {
        if (!myAnim.isPlaying)
        {
            if (isOpen)
            {       
                if(lastClip == "Open")
                    myAnim.clip = myAnim.GetClip("Close");
                else
                    myAnim.clip = myAnim.GetClip("CloseInverse");

                myAnim.Play();
                isOpen = false;
            }
            else
            {
                if ((doorDirection.z > 0 && transform.eulerAngles.y == 180) || 
                    (doorDirection.x > 0 && transform.eulerAngles.y == 90))
                {
                    myAnim.clip = myAnim.GetClip("Open");
                    lastClip = "Open";
                }
                else
                {
                    myAnim.clip = myAnim.GetClip("OpenInverse");
                    lastClip = "OpenInverse";
                }

                myAnim.Play();
                isOpen = true;
            }
        }
    }
}
