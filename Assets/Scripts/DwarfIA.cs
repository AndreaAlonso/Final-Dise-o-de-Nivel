using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DwarfIA : MonoBehaviour {

    NavMeshAgent _agent;
    AudioSource _audio;    
    Animation _anim;
    Transform _target;
    Vector3 _newTarget;
    Vector3 _newPos;
    Vector3 _originalPosition;
    float _distance;
    int _direction;
    public float angle;
    public AudioClip[] clips;
    public float distanceToSee;
    public float distanceToAttack;

    public float randomMove;
    public float timeToRandomMove;

    public float life;
    public float attack;
    public Slider slider;
    public Text text;
    

    public void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animation>();
        _target = FindObjectOfType<Hero>().transform;
        _originalPosition = this.transform.position;
    }

    // Use this for initialization
    void Start () {
        distanceToSee = 8;
        distanceToAttack = 3f;
        angle = 30;
        timeToRandomMove = Random.Range(5, 25);
        _direction = 1;

        //Setear la vida en el inspector

        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = life;
            slider.value = life;
            text.text = life.ToString();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        _newTarget = new Vector3(_target.position.x, 0, _target.position.z);
        _newPos = new Vector3(transform.position.x, 0, transform.position.z);
        _distance = Vector3.Distance(_newTarget, _newPos);

         if (((Vector3.Angle(this.transform.forward, (_target.position - this.transform.position)) < 40 &&
             _distance <= distanceToSee) || _distance <= 2) && _target.GetComponent<Hero>().life > 0)
        {          
            
            if (_distance <= distanceToAttack)
            {
                _audio.clip = clips[0];
                if (!_audio.isPlaying)
                    _audio.Play();
                _anim.Play("attack1");
                Quaternion look = Quaternion.LookRotation((_newTarget - _newPos).normalized);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, look, 0.5f);
            }               
            else
            {
                _audio.clip = clips[1];
                if (!_audio.isPlaying)
                    _audio.Play();
                _anim.Play("walk");
                _agent.SetDestination(_target.position);
            }               
        }
         else
        {

            if (Vector3.Distance(_newPos, new Vector3(_originalPosition.x, 0, _originalPosition.z)) > 4)
            {
                _anim.Play("walk");
                _agent.SetDestination(_originalPosition);
            }
            else
            {
                _anim.Play("idle break");
                this.transform.Rotate(0, ((20 + randomMove) * Time.deltaTime) * _direction, 0);
            }
        }

        timeToRandomMove -= Time.deltaTime;
        if(timeToRandomMove <= 0)
        {

            randomMove = Random.Range(40, 400);
            if (timeToRandomMove <= -1f)
            {
                _direction = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                randomMove = 0;
                timeToRandomMove = Random.Range(5, 25);
            }
        }
    }

    public void Hit(float damage)
    {
        life -= damage;
        slider.value = life;
        text.text = life.ToString();

        if (life <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            _agent = null;
            _anim.Stop();
            _anim.Play("die1");
            this.enabled = false;           
        }
    }
}
