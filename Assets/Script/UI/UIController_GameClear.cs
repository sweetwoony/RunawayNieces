using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController_GameClear : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;


	void Start () {
		this.myPlayer = GameObject.Find("Player");
		this.GetComponent<Image> ().enabled = false;
	}



	void Update () {

		bool GC01 = myPlayer.GetComponent<PlayerController>().GetKey01;
		bool GC02 = myPlayer.GetComponent<PlayerController>().GetKey02;
		bool GC03 = myPlayer.GetComponent<PlayerController>().GetKey03;
		bool GC04 = myPlayer.GetComponent<PlayerController>().GetKey04;
		bool GC05 = myPlayer.GetComponent<PlayerController>().GetKey05;

		//オブジェクト「Player」のコンポーネント「PlayerController」の変数「GetKey01～05」がtrueになったときに作動。
		if (GC01 && GC02 && GC03 && GC04 && GC05) {
			//ゲームクリアの文字表示
			this.GetComponent<Image> ().enabled = true;

			//画面タッチでゲームリスタート
			if (Input.GetMouseButtonDown (0)) {
				SceneManager.LoadScene ("GameScene");
				this.GetComponent<Image> ().enabled = false;
			}
		}

	}
}
