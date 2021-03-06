﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWeapon : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1"))
            other.GetComponent<DwarfIA>().Hit(20);
        else if (other.CompareTag("Boss1"))
            other.GetComponent<Boos1>().Hit(20);
        else if (other.CompareTag("BossFinal"))
            other.GetComponent<FinalBoss>().Hit(12);
    }
}
