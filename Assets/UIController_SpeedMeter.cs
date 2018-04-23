using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_SpeedMeter : MonoBehaviour {


	//プレイヤーのオブジェクト
	private GameObject myPlayer;

	void Start () {
		//プレイヤーのオブジェクトを取得
		this.myPlayer = GameObject.Find("Player");
	}


	void Update () {
		float SpeadBar = myPlayer.GetComponent<PlayerController>().RunSpeed;
		this.transform.localScale = new Vector3 (SpeadBar/3, 0.02f, 0);
	}
}

