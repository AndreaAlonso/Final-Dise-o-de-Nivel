using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    private Animator _anim;
    private Rigidbody _rb;
    public float shitPress;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start ()
    {
        shitPress = 0;
        _anim.SetBool("Body", true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move(Input.GetAxis("Vertical"));
        Attack();
        DoorInteraction();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Hit 1"))
        {
            _anim.SetLayerWeight(1, 0);
            _anim.SetTrigger("Attack");
        }
    }

    private void Move(float verticalMove)
    {
        _anim.SetFloat("Blend", verticalMove + shitPress);
        _rb.velocity = new Vector3(transform.forward.x,0,transform.forward.z) * (3+(Input.GetKey(KeyCode.LeftShift) ? 5:0)) * verticalMove; 
        this.transform.Rotate(0, 4f * Input.GetAxis("Horizontal"), 0);

        shitPress = Mathf.Clamp(shitPress, -1f, 1f);

        if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
            shitPress += Time.deltaTime;
        else if (shitPress > 0)
            shitPress -= Time.deltaTime;
        else
            shitPress = 0;
    }

    private void DoorInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit rh;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out rh, 3))
            {
                if (rh.collider != null && rh.transform.CompareTag("Door"))
                {
                    Doors door = rh.transform.GetComponentInParent<Doors>();
                    door.OpenOrClose((rh.transform.position - this.transform.position));
                }
            }
        }
    }
}
