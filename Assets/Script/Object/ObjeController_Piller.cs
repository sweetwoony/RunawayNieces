using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjeController_Piller : MonoBehaviour {


	void Start () {
		
	}
	

	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "PlayerTag") {
			Destroy (gameObject, 1.0f);
		}
	}
			
}
