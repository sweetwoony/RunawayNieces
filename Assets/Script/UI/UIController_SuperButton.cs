using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_SuperButton : MonoBehaviour {


	//プレイヤーのオブジェクト
	private GameObject myPlayer;
	//このオブジェクトのスプライトコンポーネントの情報を、このスクリプト内に収納する
	Image ButaImage;

	//プレイヤーのグラフィックとなるスプライトを取得
	public Sprite Buta_SuperButton01;
	public Sprite Buta_SuperButton02;
	public Sprite Buta_SuperButton03;
	public Sprite Buta_SuperButton04;
	public Sprite Buta_SuperButton05;
	public Sprite Buta_SuperButton06;
	public Sprite Buta_SuperButton07;
	public Sprite Buta_SuperButton08;
	public Sprite Buta_SuperButton09;
	public Sprite Buta_SuperButton10;
	public Sprite Buta_SuperButton11;
	public Sprite Buta_SuperButton12;
	public Sprite Buta_SuperButton13;




	void Start () {

		//プレイヤーのオブジェクトを取得
		this.myPlayer = GameObject.Find("Player");
		//このオブジェクトのスプライトを取得
		ButaImage = gameObject.GetComponent<Image>();

	}


	void Update () {

		//ダメージ後の顔面位置をリセット
		this.transform.position = new Vector2 (Screen.width / 2, Screen.height / 10);
		//衝撃耐性の溜まり方に応じた豚の表情の変化
		float ButaConditionFace = myPlayer.GetComponent<PlayerController>().ImpactResistPt;
		//オブジェクト「Player」衝突時の顔面シェイク時間を取得
		int DamageShakeTime= myPlayer.GetComponent<PlayerController>().DamagedFaceVibration;
		int ShakeRange = Random.Range (-5, 5); //顔面シェイク時のブレる方向をランダム指定
		int DFChang = myPlayer.GetComponent<PlayerController>().DamageFaceChange; //顔面シェイク時に表示される表情の情報を取得

		if (DamageShakeTime > 0) {
			this.transform.position = new Vector2 (this.transform.position.x + ShakeRange, this.transform.position.y + ShakeRange);
			//PlayerControllerのDamageFaceChangeの値により変化分岐
			if (DFChang == 1) {ButaImage.sprite = Buta_SuperButton04;}
			if (DFChang == 2) {ButaImage.sprite = Buta_SuperButton05;}
			if (DFChang == 3) {ButaImage.sprite = Buta_SuperButton06;}


		} else {
			
			if (ButaConditionFace < 20) {
				ButaImage.sprite = Buta_SuperButton01;
			}
			if (20 < ButaConditionFace && ButaConditionFace <= 40) {
				ButaImage.sprite = Buta_SuperButton09;
			}
			if (40 < ButaConditionFace && ButaConditionFace <= 60) {
				ButaImage.sprite = Buta_SuperButton10;
			}
			if (60 < ButaConditionFace && ButaConditionFace <= 80) {
				ButaImage.sprite = Buta_SuperButton11;
			}
			if (80 < ButaConditionFace && ButaConditionFace <= 100) {
				ButaImage.sprite = Buta_SuperButton12;
			}
			if (100 < ButaConditionFace) {
				ButaImage.sprite = Buta_SuperButton13;
			}
		}

	}
}
