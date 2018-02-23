using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public PlayerController playerController;
    public float waitSecont;
    public GameObject deadExplosion;
    public AudioSource audio;

	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<PlayerController>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Respawn(){
        StartCoroutine("RespawnCo");
        audio.Play();
    }

    public IEnumerator RespawnCo()
    {
        playerController.gameObject.SetActive(false);
        Instantiate(deadExplosion,playerController.transform.position,playerController.transform.rotation);
        yield return new WaitForSeconds(waitSecont);
        playerController.transform.position = playerController.RespawnPosition;
        playerController.gameObject.SetActive(true);

    }
}
