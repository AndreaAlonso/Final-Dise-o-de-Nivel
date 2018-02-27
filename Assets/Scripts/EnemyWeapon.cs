using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    Animation _anim;
    CapsuleCollider swordCollider;
    DwarfIA _dwarf;
    Boos1 _boss;

    private void Start()
    {
        _dwarf = GetComponentInParent<DwarfIA>();
        _boss = GetComponentInParent<Boos1>();
        _anim = GetComponentInParent<Animation>();
        swordCollider = GetComponent<CapsuleCollider>();
        swordCollider.enabled = false;
        StartCoroutine(num());
    }

    IEnumerator num()
    {
        while(true)
        {
            if (_anim.IsPlaying("attack1"))
            {
                swordCollider.enabled = true;
                yield return new WaitForSeconds(_anim.GetClip("attack1").length / 2f);
                swordCollider.enabled = false;
                yield return new WaitForSeconds(_anim.GetClip("attack1").length / 2f);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Hero>().Hit(_dwarf != null ? _dwarf.attack : _boss.attack);
            swordCollider.enabled = false;
        }
    }
}
