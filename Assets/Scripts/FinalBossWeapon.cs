using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossWeapon : MonoBehaviour {

    Animation _anim;
    CapsuleCollider swordCollider;
    FinalBoss _boss;

    private void Start()
    {
        _boss = GetComponentInParent<FinalBoss>();
        _anim = GetComponentInParent<Animation>();
        swordCollider = GetComponent<CapsuleCollider>();
        swordCollider.enabled = false;
        StartCoroutine(num());
    }

    IEnumerator num()
    {
        while (true)
        {
            if (_anim.IsPlaying("Hit 1"))
            {
                swordCollider.enabled = true;
                yield return new WaitForSeconds(_anim.GetClip("Hit 1").length / 2f);
                swordCollider.enabled = false;
                yield return new WaitForSeconds(_anim.GetClip("Hit 1").length / 2f);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Hero>().Hit(_boss.attack);
            swordCollider.enabled = false;
        }
    }
}
