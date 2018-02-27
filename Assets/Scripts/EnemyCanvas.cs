using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour {

    public Transform directionCamera;

	// Use this for initialization
	void Start () {
        directionCamera = FindObjectOfType<Camera>().transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3(directionCamera.position.x, 0 , directionCamera.position.z));
	}
}
