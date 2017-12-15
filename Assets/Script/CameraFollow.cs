using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float Ahead;
    public Vector3 TargetPos;
    public float smooth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TargetPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        if (player.transform.localScale.x>0f)
        {
            TargetPos = new Vector3(player.transform.position.x+Ahead, transform.position.y, transform.position.z);

        }else{
            TargetPos = new Vector3(player.transform.position.x - Ahead, transform.position.y, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, TargetPos, smooth * Time.deltaTime);



    }
}
