using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        Vector3 lookDir = player.transform.position - transform.position;
        transform.forward = new Vector3(lookDir.x, 0.0f, lookDir.z);
    }
}
