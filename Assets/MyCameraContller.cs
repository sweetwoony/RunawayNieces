using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraContller : MonoBehaviour {

	//オブジェクトplayer呼び出し
	private GameObject myPlayer;


	void Start () {
		myPlayer = GameObject.Find ("Player");
	}


	void Update () {
		
		bool VCS = myPlayer.GetComponent<PlayerController> ().ViewChangeSwitch;
		bool E_DZ = myPlayer.GetComponent<PlayerController> ().isEnd_DeadZone;
		Vector3 ViewtoPlayer = myPlayer.transform.position;

		if (E_DZ == false) {
			if (VCS == false) {   // ビュー変更の角度：初期
				this.transform.localPosition = new Vector3 (0.0f, 3.0f, -5.0f);
				this.transform.localRotation = Quaternion.Euler (20, 0, 0);
			} else if (VCS) {   //　ビュー変更の角度：見下ろし
				this.transform.localPosition = new Vector3 (0.0f, 14.0f, -5.0f);
				this.transform.localRotation = Quaternion.Euler (70, 0, 0);
			}
		} else if(E_DZ) {
				this.transform.LookAt (ViewtoPlayer);
		}
	}
}
