using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    private Animator _anim;
    public float shitPress;

	// Use this for initialization
	void Start ()
    {
        _anim = GetComponent<Animator>();
        shitPress = 0;
        _anim.SetBool("Body", true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move(Input.GetAxis("Vertical"));

        if(Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
        }

    }

    void Move(float verticalMove)
    {
        _anim.SetFloat("Blend", verticalMove + shitPress);

        shitPress = Mathf.Clamp(shitPress, -1f, 1f);

        if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
            shitPress += Time.deltaTime;
        else if (shitPress > 0)
            shitPress -= Time.deltaTime;
        else
            shitPress = 0;
    }
}
