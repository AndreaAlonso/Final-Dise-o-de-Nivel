using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    private Transform _target;
    public float upDistance;
    public float fowardDistance;
    public float fowardDistancePlus;

    private void Awake()
    {
        _target = GameObject.Find("Character").transform;
    }

    // Use this for initialization
    void Start ()
    {
        upDistance = 3f;
        fowardDistance = 3.5f;
        fowardDistancePlus = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        CameraMoveSmooth();
        CameraRotationSmooth();
        CameraWallRay();
        CameraDrawGizmos();       
    }

    private void CameraMoveSmooth()
    {
        Vector3 targetPosition = _target.position + (_target.up * upDistance) - (_target.forward * (fowardDistance + fowardDistancePlus));
        this.transform.position = Vector3.Lerp(transform.position, targetPosition, 0.08f);
    }

    private void CameraRotationSmooth()
    {
        Quaternion look = Quaternion.LookRotation(((_target.position + Vector3.up * 1.8f) - this.transform.position).normalized);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, look, 0.3f);
    }

    private void CameraWallRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, -new Vector3(transform.forward.x, 0, transform.forward.z), out hit, 2.5f) ||
            Physics.Raycast(this.transform.position, -new Vector3(transform.right.x, 0, transform.right.z), out hit, 0.5f) ||
            Physics.Raycast(this.transform.position, new Vector3(transform.right.x, 0, transform.right.z), out hit, 0.5f))
        {
            if (hit.collider != null && hit.distance < 1 && fowardDistancePlus > -2.6f)
            {               
                fowardDistancePlus -= Time.deltaTime * 8;
            }
            else if (fowardDistancePlus < 0 && hit.distance > 2f)
                fowardDistancePlus += Time.deltaTime * 4;
        }
        else if (fowardDistancePlus < 0)
            fowardDistancePlus += Time.deltaTime * 4;
    }

    private void CameraDrawGizmos()
    {
        Debug.DrawRay(this.transform.position, new Vector3(transform.forward.x, 0, transform.forward.z) * -2.5f, Color.green);
        Debug.DrawRay(this.transform.position, (new Vector3(transform.right.x, 0, transform.right.z)) * -0.5f, Color.red);
        Debug.DrawRay(this.transform.position, (new Vector3(transform.right.x, 0, transform.right.z)) * 0.5f, Color.red);
    }
}

