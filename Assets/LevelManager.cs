using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public PlayerController playerController;
    public float waitSecont;
	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<PlayerController>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Respawn(){
        StartCoroutine("RespawnCo");
    }

    public IEnumerator RespawnCo()
    {
        playerController.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitSecont);
        playerController.transform.position = playerController.RespawnPosition;
        playerController.gameObject.SetActive(true);
    }
}
