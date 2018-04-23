using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// １）スライドターンpt蓄積値に対する加減の仕方の調整
	// ２）スライドターンpt蓄積量による通常ターンとスライドターンの分岐と、それらの旋回量の調整
	// ３）ディスプレイタップに対するスイッチのＯＮ・ＯＦＦとスライドターンpt付与の調整
	// ４）ゲームオーバー処理
	// ５）壁・敵と衝突時の加算される衝撃値や反動調整
	// ６）前進に関する調整
	// ７）スーパーボタン（仮）の処理。蓄積条件は主に速度。


	// ６）８）で使用プレイヤーを移動させるコンポーネントを入れる
	private Rigidbody myRigidbody;
	//前進するための力
	private float forwardForce = 12.0f;

	//左スライドターン猶予
	private int LSlideTurnCount = 0;
	//右スライドターン猶予
	private int RSlideTurnCount = 0;
	//　１）で使用　スライドターンポイント蓄積上限
	private int SlideTurnLimit = 35;
	//　２）で使用　通常ターンの旋回力
	private float TurnForce = 60.0f;
	//　２）で使用　スライドターンの旋回力
	private float SlideTurnForce = 500.0f;
	//　２）で使用　通常ターンorスライドターン　分岐ポイント
	private int SlideTurnBranch = 23;
	//　３）で使用　スライドターン実行のためのポイント付与
	private int SlideTurnTriggerPt = 20;
	//　３）で使用　左ボタン押下の判断
	private bool isLButtonDown = false;
	//　３）で使用　右ボタン押下の判断
	private bool isRButtonDown = false;
	// ４）で使用　ゲームオーバー判定
	public bool isEnd_DeadZone;
	// ４）で使用　動きを減衰させる係数
	private float coefficient = 0.95f;
	// ５）で使用　プレイヤーの衝撃値
	public float ImpactResistPt;
	// ５）で使用　ダメージによる顔面シェイク時に表示される表情をランダム指定。ここでは０
	public int DamageFaceChange = 0;
	// ５）で使用　衝撃値に影響する接触時の顔面振動時間
	public int DamagedFaceVibration = 0;
	// ５）で使用　衝撃時に重力操作による反動増加が付与される時間
	public float DamageRecoilTime = 0;
	// ６）で使用　前進ＯＮ・ＯＦＦスイッチ
	private bool WalkForwardSwitch = false;
	// ７）で使用　スーパーボタン（仮）蓄積値
	public float SuperButtonCharge = 0;
	// ７）で使用　スーパーボタン（仮）秒間蓄積量
	public float SuperButtonChargeGain = 0.2f;
	// ７）で使用　スーパーボタン（仮）発動時の秒間消費量
	public float SuperButtonChargedrain = 1.0f;
	// ７）で使用　スーパーボタン押下の判断
	private bool isSuperButtonDown = false;
	// ７）で使用　プレイヤーの速度ベクトルを収納する
	public float RunSpeed = 0;

	void Start () {
		// ６）Rigidbodyコンポーネントを取得
		this.myRigidbody = this.GetComponent<Rigidbody>();

		// ４）UIスクリプト「UIController_GameOver」の、publicなbool変数「GameOverSensor」を直接操作。
		this.isEnd_DeadZone = false; //リスタート時のゲームオーバー判定除去用

		// ５）プレイヤーの衝撃耐性の初期化
		this.ImpactResistPt = 0;
	}
		


	void Update (){


		//　４）DeadZoneによるゲームオーバーならプレイヤーの動きを減衰する
		if (this.isEnd_DeadZone) {
			this.forwardForce *= this.coefficient;
			this.TurnForce *= this.coefficient;
			this.SlideTurnForce *= this.coefficient;
		}

		// ５）プレイヤー衝撃値の時限回復
		if (100 <= ImpactResistPt) {
			this.ImpactResistPt -= 1.0f;
		} else if (60 <= this.ImpactResistPt && this.ImpactResistPt < 1000) {
			this.ImpactResistPt -= 0.08f;
		} else if (0 < this.ImpactResistPt) {
			this.ImpactResistPt -= 0.07f;
		} else {
			this.ImpactResistPt = 0;
		}


		// ５．１）衝撃耐性の蓄積によって反動が変動

		if (0 < DamageRecoilTime) {
			DamageRecoilTime -= 1;
		} else if (DamageRecoilTime <= 0) {
			this.GetComponent<Rigidbody> ().mass = 0.8f;
			DamageRecoilTime = 0;
		}



		// ５）DamagedFaceVibrationの数値を減少させる。０になると振動終了
		if (DamagedFaceVibration > 0) {
			DamagedFaceVibration -= 1;
		} else {
			DamageFaceChange = 0; // 顔面変更をリセット
		}



		//　１）スライドターンの蓄積上限処理
		if (this.LSlideTurnCount > SlideTurnLimit) {
			this.LSlideTurnCount = SlideTurnLimit - 1;
		} else if (this.LSlideTurnCount <= 0) {
			this.LSlideTurnCount = 0;
		} else {
			this.LSlideTurnCount -= 1;
		}


		if (this.RSlideTurnCount > SlideTurnLimit) {
			this.RSlideTurnCount = SlideTurnLimit - 1;
		} else if (this.RSlideTurnCount <= 0) {
			this.RSlideTurnCount = 0;
		} else {
			this.RSlideTurnCount -= 1;
		}
			



		// ７）プレイヤーの速度ベクトルをコンポーネントから取得
		RunSpeed = myRigidbody.velocity.magnitude;

		// ７）プレイヤーが接地中 且つ 速度が一定以上 且つ　スーパーボタン（仮）発動中でない　ならスーパーボタン（仮）蓄積量増加
		if (8 < RunSpeed && SuperButtonCharge < 100 && isSuperButtonDown == false && WalkForwardSwitch) {
			SuperButtonCharge += SuperButtonChargeGain;
		} else if (0 < SuperButtonCharge && isSuperButtonDown) {
			// ７）スーパーボタン（仮）発動中ならば蓄積量を消費し続ける
			SuperButtonCharge -= SuperButtonChargedrain;
		}

		// ７）スーパーボタン発動時か否かの移動速度処理
		if (RunSpeed <= 12 && isSuperButtonDown == false && WalkForwardSwitch) {
			// ６）前進スイッチ「WalkForwardSwitch」がtrueで、最高速度に達していないなら前進するための重力が働く
			this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);
		} else if (isSuperButtonDown) {
			// ７）スーパーボタン発動時の速度
			this.myRigidbody.AddForce (this.transform.forward * this.forwardForce * 2);
		}

		// ７）スーパーボタン（仮）発動し、蓄積量が０になったら通常モードに戻る
		if (isSuperButtonDown && SuperButtonCharge <= 0) {
			SuperButtonCharge = 0;
			isSuperButtonDown = false;
			GetComponent<ParticleSystem> ().Stop ();
		}
	}




	//　３）ボタン入力によるスイッチ・スライドターンポイントの加算

	//左ボタンを押し続けた場合の処理
	public void GetMyLeftButtonDown(){
		this.LSlideTurnCount += this.SlideTurnTriggerPt;
		this.isLButtonDown = true;
	}
	//左ボタンを放した場合の処理
	public void GetMyLeftButtonUp(){
		this.isLButtonDown = false;
	}


	//右ボタンを押し続けた場合の処理
	public void GetMyRightButtonDown(){
		this.RSlideTurnCount += this.SlideTurnTriggerPt;
		this.isRButtonDown = true;
	}
	//右ボタンを放した場合の処理
	public void GetMyRightButtonUp(){
		this.isRButtonDown = false;
	}

	// ７）スーパーボタン（仮）を押したときの処理
	public void GetMySuperButtonDown(){
		//スーパーボタン（仮）満タンならtrue。エフェクトが出る
		if (100 <= SuperButtonCharge) {
			GetComponent<ParticleSystem>().Play();
			this.isSuperButtonDown = true;
		}
	}
		

	//　４）トリガーモードで他のオブジェクトと触れた場合の処理
	void OnTriggerExit(Collider other){
		//　４）デッドゾーン通過（落下）
		if (other.gameObject.tag == "GameOverTag_DeadZone") {
			//UIスクリプト「UIController_GameOver」の、publicなbool変数「GameOverSensor」を直接操作。
			this.isEnd_DeadZone = true;
		}
	}

	// ５）衝突対象よって衝撃値の設定値を変える。ダメージでの顔振動時間の設定も入力する。
	// 壁・柱
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "DamageTag_Obstacle") {
			DamagedFaceVibration = 0; // ダメージ顔振動を予めリセット。
			DamagedFaceVibration = 15; // 数値を入れて顔ダメージの振動開始
			DamageFaceChange = Random.Range(1,4);
			ImpactResistPt += 15;
			if (100 <= this.ImpactResistPt) {
				this.GetComponent<Rigidbody> ().mass = 0.2f;
				this.DamageRecoilTime = 10;
			}
		}
	}


	void OnCollisionStay(Collision collision){
		// ６）「GroundTag」と接触＝接地しているなら前進・旋回が出来る
		if (collision.gameObject.tag == "GroundTag") {
			WalkForwardSwitch = true;
			this.GetComponent<Animator> ().SetTrigger ("walkTrigger");

			//　２）通常ターンorスライドターン　ポイントによる分岐
			//左旋回
			if (this.isLButtonDown) {
				if (LSlideTurnCount > SlideTurnBranch) {
					this.transform.Rotate (0.0f, -SlideTurnForce * Time.deltaTime, 0.0f);
					ImpactResistPt += 1.0f; //さりげなく衝撃値加算
				} else if (LSlideTurnCount <= SlideTurnBranch) {
					this.transform.Rotate (0.0f, -TurnForce * Time.deltaTime, 0.0f);
				}
			}

			//右旋回
			if (this.isRButtonDown) {
				//スライドターン猶予の検知。通常ターンとスライドターンの分岐
				if (RSlideTurnCount > SlideTurnBranch) {
					this.transform.Rotate (0.0f, SlideTurnForce * Time.deltaTime, 0.0f);
					ImpactResistPt += 1.0f; //さりげなく衝撃値加算
				} else if (RSlideTurnCount <= SlideTurnBranch) {
					this.transform.Rotate (0.0f, TurnForce * Time.deltaTime, 0.0f);
				}
			}
		}
	}

	void OnCollisionExit(Collision collision){
		// ６）GroundTag」と離れているなら前進・旋回するスイッチを切る
		if (collision.gameObject.tag == "GroundTag") {
			WalkForwardSwitch = false;
		}
	}
		


}

