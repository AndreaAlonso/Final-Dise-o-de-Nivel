using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Hero : MonoBehaviour {

    private Animator _anim;
    private Rigidbody _rb;
    public float shitPress;
    public GameObject pressF;
    public GameObject needKey;
    public int keyCant;
    public bool canOpenDoor;
    public float life;
    public Slider lifeSlider;
    public Text texto;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        pressF = GameObject.Find("Canvas").transform.Find("PressFPanel").gameObject;
        needKey = GameObject.Find("Canvas").transform.Find("KeyInformationPanel").gameObject;
    }
    // Use this for initialization
    void Start ()
    {
        shitPress = 0;
        life = 100f;
        lifeSlider.maxValue = 100;
        lifeSlider.minValue = 0;
        lifeSlider.value = 100;

        _anim.SetBool("Body", true);
        Cursor.visible = false;
        canOpenDoor = false;
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
          //  _anim.SetLayerWeight(1, 0);
            _anim.SetTrigger("Attack");
        }
    }

    private void Move(float verticalMove)
    {
        _anim.SetFloat("Blend", verticalMove + shitPress);
        _rb.velocity = 
            new Vector3
            (
            ((transform.forward.x * verticalMove) * (3 + (Input.GetKey(KeyCode.LeftShift) ? 3 : 0))) + (transform.right.x * (Input.GetAxis("Horizontal"))),
            _rb.velocity.y,
            ((transform.forward.z * verticalMove) * (3 + (Input.GetKey(KeyCode.LeftShift) ? 3 : 0))) + (transform.right.z * (Input.GetAxis("Horizontal")))
            ) ; 

        this.transform.Rotate(0,3*Input.GetAxis("Mouse X"), 0);

        shitPress = Mathf.Clamp(shitPress, 0f, 1f);

        if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
            shitPress += Time.deltaTime;
        else if (shitPress > 0)
            shitPress -= Time.deltaTime;
        else
            shitPress = 0;
    }

    private void DoorInteraction()
    {
        RaycastHit rh;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out rh, 3, 1 << LayerMask.NameToLayer("OpenClose")))
        {

            if (rh.collider)
            {
                Doors door = rh.transform.GetComponentInParent<Doors>();
                Chest chest = rh.transform.GetComponent<Chest>();

                if(door != null && door.needKey)
                {
                 needKey.gameObject.SetActive(true);
                 canOpenDoor = false;
                   if (keyCant > 0)
                   {
                        needKey.gameObject.SetActive(false);
                        --keyCant;
                       door.needKey = false;
                       canOpenDoor = true;
                   }
                }
                else
                {
                    canOpenDoor = true;
                    pressF.gameObject.SetActive(true);
                }

            }           
        }
        else
        {
            if (pressF.activeSelf || needKey.activeSelf)
            {
                pressF.gameObject.SetActive(false);
                needKey.gameObject.SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.F))
        {           
                if (rh.collider != null && canOpenDoor)
                {
                    OpenClose openClose = rh.transform.GetComponentInParent<OpenClose>();
                    if(openClose!=null)
                        openClose.OpenClose((rh.transform.position - this.transform.position));
                }            
        }
    }

    public void Hit(float damage)
    {
        life -= damage;
        lifeSlider.value = life;
        texto.text = life.ToString();
    }
}
