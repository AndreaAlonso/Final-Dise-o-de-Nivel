using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boos1 : MonoBehaviour
{

    NavMeshAgent _agent;
    Animation _anim;
    Transform _target;
    Vector3 _newTarget;
    Vector3 _newPos;
    float _distance;
    public float angle;
    public bool canAttack;

    public float distanceToAttack;



    public void Awake()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        _newTarget = new Vector3(_target.position.x, 0, _target.position.z);
        _newPos = new Vector3(transform.position.x, 0, transform.position.z);
        _distance = Vector3.Distance(_newTarget, _newPos);

        if (canAttack)
        {

            if (_distance <= distanceToAttack)
            {
                _anim.Play("Attack1");
                Quaternion look = Quaternion.LookRotation((_newTarget - _newPos).normalized);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, look, 0.5f);
            }
            else
            {
                _anim.Play("Walk");
                _agent.SetDestination(_target.position);
            }
        }
        else
        {
              _anim.Play("Standby");
        }
    }
}

