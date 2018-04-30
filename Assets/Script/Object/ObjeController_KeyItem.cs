using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjeController_KeyItem : MonoBehaviour {



	void Start () {

	}

	void Update () {
		//オブジェクトを回転させ続ける
		this.transform.Rotate (0.0f, 100.0f * Time.deltaTime, 0.0f);
	}

	//Playerと衝突でアイテム消滅
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "PlayerTag") {
			Destroy (gameObject);
		}
	}


}
