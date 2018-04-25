using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// １）前進に関する調整
	// ２．１）ターン関連；旋回入力ＯＮ・ＯＦＦ、各旋回量、
	// ２．２）ターン関連；スライドターンpt蓄積値関連
	// ３）スコア関連
	// ４）キーアイテム関連
	// ５）スーパーボタン（仮）の処理。蓄積条件は主に速度。
	// ６）ゲームオーバー処理
	// ７）壁・敵と衝突時の加算される衝撃値や反動調整


	// プレイヤーを移動させるコンポーネントを入れる
	private Rigidbody myRigidbody;

	// １）で使用　前進ＯＮ・ＯＦＦスイッチ
	private bool WalkForwardSwitch = false;
	// １）で使用　前進するための力
	private float forwardForce = 12.0f;
	// RigidBody.Massの初期値
	private float myMass = 1.0f;

	// ２．１）で使用　左通常ターン実行の判断
	private bool LNormalTurnSwitch = false;
	// ２．１）で使用　右通常ターン実行の判断
	private bool RNormalTurnSwitch = false;
	// ２．１）で使用　左スライドターン実行の判断
	private bool LSlideTurnSwitch = false;
	// ２．１）で使用　右スライドターン実行の判断
	private bool RSlideTurnSwitch = false;
	// ２．１）で使用　通常ターンの旋回力
	private float TurnForce = 60.0f;
	// ２．１）で使用　スライドターンの旋回力
	private float SlideTurnForce = 80.0f;
	// ２．１）で使用　左スライドターンを実行中の経過時間
	private float LSlideTurnTime = 0.0f;
	// ２．１）で使用　右スライドターンを実行中の経過時間
	private float RSlideTurnTime = 0.0f;
	// ２．１）で使用　スライドターン終了を終了させる時間
	private float duration = 0.25f;


	// ２．２）で使用　通常ターンorスライドターン　分岐ポイント
	private float SlideTurnBranch = 25;
	// ２．２）で使用　スライドターンptポイント付与の値
	private float SlideTurnTriggerPt = 20;
	// ２．２）で使用　左スライドターンpt蓄積値
	private float  LSlideTurnCount = 0;
	// ２．２）で使用　右スライドターンpt蓄積値
	private float RSlideTurnCount = 0;

	// ３）で使用　スコア獲得点を格納
	public float ScorePt = 0;

	// ４）で使用　取得したキーアイテムの情報を格納
	public bool GetKey01 = false;
	public bool GetKey02 = false;
	public bool GetKey03 = false;
	public bool GetKey04 = false;
	public bool GetKey05 = false;


	// ５）で使用　スーパーボタン（仮）蓄積値
	public float SuperButtonCharge = 0;
	// ５）で使用　スーパーボタン（仮）秒間蓄積量
	public float SuperButtonChargeGain = 0.2f;
	// ５）で使用　スーパーボタン（仮）発動時の秒間消費量
	public float SuperButtonChargedrain = 1.5f;
	// ５）で使用　スーパーボタン押下の判断
	private bool isSuperButtonDown = false;
	// ５）で使用　プレイヤーの速度ベクトルを収納する
	public float RunSpeed = 0;


	// ６）で使用　プレイヤーの衝撃値
	public float ImpactResistPt;
	// ６）で使用　ダメージによる顔面シェイク時に表示される表情をランダム指定。ここでは０
	public int DamageFaceChange = 0;
	// ６）で使用　衝撃値に影響する接触時の顔面振動時間
	public int DamagedFaceVibration = 0;
	// ６）で使用　衝撃時に重力操作による反動増加が付与される時間
	public float DamageRecoilTime = 0;
	// ６）で使用　衝撃耐性が一定以上で反動増加する設定をＯＮにするスイッチ
	private bool DamageRicoilSwitch = false;

	// ７）で使用　ゲームオーバー判定
	public bool isEnd_DeadZone;
	// ７）で使用　動きを減衰させる係数
	private float coefficient = 0.95f;




	void Start () {
		// Rigidbodyコンポーネントを取得
		this.myRigidbody = this.GetComponent<Rigidbody>();

		// ４）UIスクリプト「UIController_GameOver」の、publicなbool変数「GameOverSensor」を直接操作。
		this.isEnd_DeadZone = false; //リスタート時のゲームオーバー判定除去用

		// ６）プレイヤーの衝撃耐性の初期化
		this.ImpactResistPt = 0;
		// ６）プレイヤーの質量を初期化
		this.myRigidbody.mass = myMass;
	}
		


	void Update (){

		// ２．２）スライドターン蓄積値の時間減少と下限処理
		if (this.LSlideTurnCount <= 0) {this.LSlideTurnCount = 0;} else {this.LSlideTurnCount -= 1;}   //左旋回
		if (this.RSlideTurnCount <= 0) {this.RSlideTurnCount = 0;} else {this.RSlideTurnCount -= 1;}   //右旋回

		// ２）左側の通常ターンorスライドターン分岐スイッチOn時の処理
		if (this.LSlideTurnSwitch) { //２．２）分岐：スライドターン
			this.transform.Rotate (0.0f, -SlideTurnForce / duration * Time.deltaTime, 0.0f); // ２．１）スライドターン実行及び旋回力
			LSlideTurnTime += Time.deltaTime;   // ２．１）スライドターン経過時間に加算
			ImpactResistPt += 0.5f;　　　// ５）さりげなく衝撃値加算
			if (LSlideTurnTime > duration) {
				LSlideTurnCount = 0;　　// ２．２）スライドターン蓄積値のリセット
				LSlideTurnTime = 0;
				LSlideTurnSwitch = false;
			} 
		} else if (this.LNormalTurnSwitch) {
				this.transform.Rotate (0.0f, -TurnForce * Time.deltaTime, 0.0f);　　// ２．１）通常ターン実行及び旋回力
		}

		// ２）右側の通常ターンorスライドターン分岐スイッチOn時の処理
		if (this.RSlideTurnSwitch) { //２．２）分岐：スライドターン
			this.transform.Rotate (0.0f, SlideTurnForce / duration * Time.deltaTime, 0.0f); // ２．１）スライドターン実行及び旋回力
			RSlideTurnTime += Time.deltaTime;   // ２．１）スライドターン経過時間に加算
			ImpactResistPt += 0.5f;　　　// ５）さりげなく衝撃値加算
			if (RSlideTurnTime > duration) {
				RSlideTurnCount = 0;　　// ２．２）スライドターン蓄積値のリセット
				RSlideTurnTime = 0;
				RSlideTurnSwitch = false;
			} 
		} else if (this.RNormalTurnSwitch) {
			this.transform.Rotate (0.0f, TurnForce * Time.deltaTime, 0.0f);　　// ２．１）通常ターン実行及び旋回力
		}
			

		//　４）DeadZoneによるゲームオーバーならプレイヤーの動きを減衰する
		if (this.isEnd_DeadZone) {
			this.forwardForce *= this.coefficient;
			this.TurnForce *= this.coefficient;
			this.SlideTurnForce *= this.coefficient;
		}


		// ６）プレイヤー衝撃値の段階的時限回復量
		if (100 <= ImpactResistPt) {
			this.ImpactResistPt -= 1.0f;
		} else if (60 <= this.ImpactResistPt && this.ImpactResistPt < 1000) {
			this.ImpactResistPt -= 0.08f;
		} else if (0 < this.ImpactResistPt) {
			this.ImpactResistPt -= 0.07f;
		} else {
			this.ImpactResistPt = 0;
		}

	
		// ６）衝撃耐性の蓄積によって反動が変動
		if (0 < DamageRecoilTime && DamageRicoilSwitch) {
			this.DamageRecoilTime -= 1;
		} else if (DamageRecoilTime <= 0 && DamageRicoilSwitch) {
			DamageRecoilTime = 0;
			DamageRicoilSwitch = false;
			this.myRigidbody.mass = myMass; //Playerの質量を元に戻す
		}



		// ６）DamagedFaceVibrationの数値を減少させる。０になると振動終了
		if (DamagedFaceVibration > 0) {
			DamagedFaceVibration -= 1;
		} else {
			DamageFaceChange = 0; // 顔面変更をリセット
		}
			


		// ５）プレイヤーの速度ベクトルをコンポーネントから取得
		RunSpeed = myRigidbody.velocity.magnitude;

		// ５）プレイヤーが接地中 且つ 速度が一定以上 且つ　スーパーボタン（仮）発動中でない　ならスーパーボタン（仮）蓄積量増加
		if (8 < RunSpeed && SuperButtonCharge < 100 && isSuperButtonDown == false && WalkForwardSwitch) {
			SuperButtonCharge += SuperButtonChargeGain;
		} else if (0 < SuperButtonCharge && isSuperButtonDown) {
			//スーパーボタン（仮）発動中ならば蓄積量を消費し続ける
			SuperButtonCharge -= SuperButtonChargedrain;
		}
			

		// ５）スーパーボタン発動時か否かの移動速度処理
		// １）前進スイッチ「WalkForwardSwitch」がtrue、最高速度に達していない、DamageRecoilTimeが0以下なら前進するための重力が働く
		if (RunSpeed <= 12 && isSuperButtonDown == false && WalkForwardSwitch && DamageRecoilTime <= 0) {
			this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);
			//DamageRecoilTimeが0　且つ　スーパーボタンが押されていたらスーパーボタン発動
		} else if (isSuperButtonDown && DamageRecoilTime <= 0) {
			// ５）スーパーボタン発動時の速度
			this.myRigidbody.AddForce (this.transform.forward * this.forwardForce * 2);
		}


		// ５）スーパーボタン（仮）発動し、蓄積量が０になったら通常モードに戻る
		if (isSuperButtonDown && SuperButtonCharge <= 0) {
			SuperButtonCharge = 0;
			isSuperButtonDown = false;
			GetComponent<ParticleSystem> ().Stop ();
		}
	}
		



	// ２）ボタン入力によるスイッチ・スライドターンポイントの加算
	// ２）左ボタンを押した時の旋回スイッチ切り替えを行い、オブジェクト「UI_LeftTurnButton」に渡すための処理
	public void GetMyLeftButtonDown(){
		this.LSlideTurnCount += this.SlideTurnTriggerPt;   // ２．２）左スライドターン蓄積値に加算
		if (LSlideTurnCount > SlideTurnBranch) {   //２．１）通常orスライドターンの分岐
			LSlideTurnCount = 0; // ２．２）スライドターン蓄積値をリセット
			this.LSlideTurnSwitch = true;
		}
			this.LNormalTurnSwitch = true;
	}


	// ２）左ボタンを放した時の処理を行い、オブジェクト「UI_LeftTurnButton」に渡すための処理
	public void GetMyLeftButtonUp(){
		this.LNormalTurnSwitch = false; // ２．１）通常ターン実行スイッチＯＦＦ
	}
		
	// ２）右ボタンを押した時の旋回スイッチ切り替えを行い、オブジェクト「UI_RightTurnButton」に渡すための処理
	public void GetMyRightButtonDown(){
		this.RSlideTurnCount += this.SlideTurnTriggerPt;   // ２．２）左スライドターン蓄積値に加算
		if (RSlideTurnCount > SlideTurnBranch) {   //２．１）通常orスライドターンの分岐
			RSlideTurnCount = 0; // ２．２）スライドターン蓄積値をリセット
			this.RSlideTurnSwitch = true;
		} 
		this.RNormalTurnSwitch = true;
	}

	// ２）左ボタンを放した時の処理を行い、オブジェクト「UI_LeftTurnButton」に渡すための処理
	public void GetMyRightButtonUp(){
		this.RNormalTurnSwitch = false; // ２．１）通常ターン実行スイッチＯＦＦ
	}



	void OnCollisionStay(Collision collision){
		// １）「GroundTag」と接触＝接地しているなら前進する
		if (collision.gameObject.tag == "GroundTag") {
			WalkForwardSwitch = true;
		}
	}
	


	// １）「GroundTag」と離れているなら前進・旋回するスイッチを切る
	void OnCollisionExit(Collision collision){
		if (collision.gameObject.tag == "GroundTag") {
			WalkForwardSwitch = false;
		}
	}
		

	// ５）スーパーボタン（仮）を押したときの処理
	public void GetMySuperButtonDown(){
		//スーパーボタン（仮）満タンならtrue。エフェクトが出る
		if (100 <= SuperButtonCharge) {
			GetComponent<ParticleSystem>().Play();
			this.isSuperButtonDown = true;
		}
	}
		


	void OnCollisionEnter(Collision collision){


		// ６）不利益のあるオブジェクトと衝突した時の設定
		//衝突対象よって衝撃値の設定値を個別で変える。ダメージでの顔振動時間の設定も入力する。

		// 柱
		if (collision.gameObject.tag == "DamageTag_Wall") {
			DamagedFaceVibration = 0; // ダメージ顔振動を予めリセット。
			DamagedFaceVibration = 15; // 数値を入れて顔ダメージの振動開始
			DamageFaceChange = Random.Range(1,4);
			ImpactResistPt += 15;
			if (100 <= this.ImpactResistPt) {
				this.DamageRicoilSwitch = true; //update()に渡すためのスイッチ
				this.myRigidbody.mass = 0.05f; //物体の質量が変化
				this.DamageRecoilTime = 20; //ピヨッてる時間
			} else if (60 <= this.ImpactResistPt) {
				this.DamageRicoilSwitch = true; //update()に渡すためのスイッチ
				this.DamageRecoilTime = 50; //ピヨッてる時間
			}
		}
		// 壁
		if (collision.gameObject.tag == "DamageTag_Piller") {
			DamagedFaceVibration = 0; // ダメージ顔振動を予めリセット。
			DamagedFaceVibration = 10; // 数値を入れて顔ダメージの振動開始
			DamageFaceChange = Random.Range(1,4);
			ImpactResistPt += 10;
		}



	}




	void OnTriggerEnter(Collider other){
		// ３）利益のあるオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_ScoreItem") {
			ScorePt += 10;   // ３）スコア獲得
			SuperButtonCharge += 5;  // ５）スーパーボタン上昇
		}


		// ４）クリアに必要なオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_KeyItem01") {
			GetKey01 = true;   // ４）キー獲得
		}
		// ４）クリアに必要なオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_KeyItem02") {
			GetKey02 = true;   // ４）キー獲得
		}
		// ４）クリアに必要なオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_KeyItem03") {
			GetKey03 = true;   // ４）キー獲得
		}
		// ４）クリアに必要なオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_KeyItem04") {
			GetKey04 = true;   // ４）キー獲得
		}
		// ４）クリアに必要なオブジェクトと衝突した時の設定
		if (other.gameObject.tag == "GetItemTag_KeyItem05") {
			GetKey05 = true;   // ４）キー獲得
		}
	}



	//　７）ゲームオーバー処理
	void OnTriggerExit(Collider other){
		//　７）死因＝デッドゾーン通過（落下）
		if (other.gameObject.tag == "GameOverTag_DeadZone") {
			//UIスクリプト「UIController_GameOver」の、publicなbool変数「GameOverSensor」を直接操作。
			this.isEnd_DeadZone = true;
		}
	}





		


}

