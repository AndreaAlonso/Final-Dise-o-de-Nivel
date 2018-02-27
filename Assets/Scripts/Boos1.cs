using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boos1 : MonoBehaviour
{
    AudioSource _audio;
    public AudioClip[] clips;
    NavMeshAgent _agent;
    Animation _anim;
    Transform _target;
    Vector3 _newTarget;
    Vector3 _newPos;
    float _distance;
    public float angle;
    public bool canAttack;

    public float distanceToAttack;

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
    }

    // Use this for initialization
    void Start()
    {
        canAttack = false;
        distanceToAttack = 3f;
        angle = 30;
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 100;

        text.text = 100.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _newTarget = new Vector3(_target.position.x, 0, _target.position.z);
        _newPos = new Vector3(transform.position.x, 0, transform.position.z);
        _distance = Vector3.Distance(_newTarget, _newPos);

        if (canAttack && _target.GetComponent<Hero>().life > 0)
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
                _anim.Play("Walk");
                _agent.SetDestination(_target.position);
            }
        }
        else
        {
              _anim.Play("Standby");
        }
    }

    public void Hit(float damage)
    {
        life -= damage;
        slider.value = life;
        if(text != null)
        text.text = life.ToString();

        if (life <= 0)
        {
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            _agent = null;
            _anim.Stop();
            _anim.Play("Death1");
            this.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}

