using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController_GameOver : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;


	void Start () {
		this.myPlayer = GameObject.Find("Player");
		this.GetComponent<Image> ().enabled = false;
	}



	void Update () {

		bool GameOverSensor = myPlayer.GetComponent<PlayerController>().isEnd_DeadZone;

		//オブジェクト「Player」のコンポーネント「PlayerController」の変数「GameOverSensor」がtrueになったときに作動。
		if (GameOverSensor) {
			//ゲームオーバーの文字表示
			this.GetComponent<Image> ().enabled = true;

			//画面タッチでゲームリスタート
			if (Input.GetMouseButtonDown (0)) {
				SceneManager.LoadScene ("GameScene");
				this.GetComponent<Image> ().enabled = false;
			}
		}
			
	}
}
