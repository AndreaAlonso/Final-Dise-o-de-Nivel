using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour {

    public Transform directionToHero;

	// Use this for initialization
	void Start () {
        directionToHero = FindObjectOfType<Hero>().transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(directionToHero.position);
	}
}
