using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraContller : MonoBehaviour {
	
	//プレイヤーのオブジェクト
	private GameObject myPlayer;


	void Start () {
		//プレイヤーのオブジェクトを取得
		this.myPlayer = GameObject.Find("Player");
	}


		void Update () {

		bool GameOverSensor = myPlayer.GetComponent<PlayerController>().isEnd_DeadZone;
		//UIスクリプト「UIController_GameOver」のpublic static な変数「GameOverSensor」がfalseである限りカメラ追従
		if (GameOverSensor == false) {
			//プレイヤーの位置に合わせてカメラの位置を移動。X,Y軸はプレイヤーに追従。
			this.transform.position = new Vector3 (myPlayer.transform.position.x, myPlayer.transform.position.y + 15, myPlayer.transform.position.z - 5);
		}
	}
}
