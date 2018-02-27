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
    public Text condicion;
    public bool activeDefense;
    GameController gameController;

    public Text lifeText;
    public Text keyText;
    public Text cointsText;
    public int coins;
    public bool checkPoint;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        gameController = FindObjectOfType<GameController>();
        pressF = GameObject.Find("Canvas").transform.Find("PressFPanel").gameObject;
        needKey = GameObject.Find("Canvas").transform.Find("KeyInformationPanel").gameObject;
    }
    // Use this for initialization
    void Start ()
    {
        lifeText.text = 5.ToString();
        shitPress = 0;
        life = 100f;
        lifeSlider.maxValue = 100;
        lifeSlider.minValue = 0;
        lifeSlider.value = 100;
        keyCant = 0;
        _anim.SetBool("Body", true);
        Cursor.visible = false;
        canOpenDoor = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (life > 0)
        {
            Move(Input.GetAxis("Vertical"));
            Attack();
            DoorInteraction();

            if(Input.GetKeyDown(KeyCode.Alpha1) && (100 - life) >= 30 && int.Parse(lifeText.text) > 0)
            {
                life += 30;
                lifeText.text = ( (int.Parse(lifeText.text)) - 1 ).ToString();
                lifeSlider.value = life;
                texto.text = life.ToString();
            }
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButton(1))
        {
            _anim.SetBool("Defense", true);
            activeDefense = true;         
        }        
        else if (Input.GetMouseButtonDown(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Hit 1"))
        {
          //  _anim.SetLayerWeight(1, 0);
            _anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonUp(1))
        {
            _anim.SetBool("Defense", false);
            activeDefense = false;
        }

    }

    private void Move(float verticalMove)
    {
        _anim.SetFloat("Blend", verticalMove + shitPress);
        _rb.velocity =
            new Vector3
            (((transform.forward.x * verticalMove) * (3 + (Input.GetKey(KeyCode.LeftShift) ? 3 : 0))),
            _rb.velocity.y,
            ((transform.forward.z * verticalMove) * (3 + (Input.GetKey(KeyCode.LeftShift) ? 3 : 0))));

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
                        keyText.text = keyCant.ToString();
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

                StartCoroutine(visibleCoinAndKey());
                
                }            
        }
    }

    IEnumerator visibleCoinAndKey()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var item in Physics.OverlapSphere(this.transform.position, 2))
        {
            if (item.CompareTag("Coins"))
            {
                Destroy(item.gameObject);
                coins += 100;
                cointsText.text = "Coins: " + coins;
            }

            if (item.CompareTag("Keys"))
            {
                Destroy(item.gameObject);
                ++keyCant;
                keyText.text = keyCant.ToString();
            }
        }
    }

    public bool Hit(float damage)
    {
        if (!activeDefense)
        {
            life -= damage;
            lifeSlider.value = life;
            texto.text = life.ToString();

            if(life <= 0)
            {
                life = 0;
                condicion.text = "Perdiste\n Presione una tecla para continuar";
                _anim.SetBool("Death", true);               
                StartCoroutine(backLife());
            }
        }

        return activeDefense;
    }

    IEnumerator backLife()
    {
        yield return new WaitUntil(()=>Input.anyKey);
        if (!checkPoint) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        else
        {
            gameController.GetHeroInformation(this);
            lifeSlider.value = life;
            texto.text = life.ToString();
            condicion.text = "";
            _anim.SetBool("Death", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] col = gameController.colliders.Values.ToArray();
        if (other == col[0])
        {
            gameController.SetRespawn("Spawn1", this);
        }
        else if (other == col[1])
        {
            gameController.SetRespawn("Spawn2", this);
        }
    }
}
