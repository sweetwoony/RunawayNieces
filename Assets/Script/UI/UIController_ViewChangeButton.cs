using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIController_ViewChangeButton: MonoBehaviour {
	//８）ビューチェンジエリアで行った動作に関するスクリプト

	//８）ビューチェンジのスイッチ
	public bool ViewChangeSwitch = false;

	public GameObject MainCamera;
	public float MoveSpeed = 0.01f;
	//８）ビューチェンジエリアでのスワイプに使う変数　タッチした時の場所
	private Vector3 ViewChange_TouchStartPos;
	//８）ビューチェンジエリアでのスワイプに使う変数　タッチを終えた時の場所
	private Vector3 ViewChange_TouchEndPos;



	void Start () {
		
	}
	
	void Update () {
		

	}




	// ８）ビューチェンジエリアをタップし、ビュー変更をしたときの処理
	public void GetMyViewChangeButtonClick(){
		if (ViewChangeSwitch == false) {
			ViewChangeSwitch = true;
		} else if(ViewChangeSwitch == true){
			ViewChangeSwitch = false;
		}
	}



	public void GetMyViewChangeButtonFlick(){
		int touchCount = Input.touches.Count (t => t.phase != TouchPhase.Ended && t.phase != TouchPhase.Canceled);
		Debug.Log (touchCount);
		if (touchCount == 1) {
			Touch t = Input.touches.First ();
			switch (t.phase) {
			case TouchPhase.Moved:
				float xAngle = t.deltaPosition.y * MoveSpeed * 10;
				float yAngle = -t.deltaPosition.y * MoveSpeed * 10;
				float zAngle = 0;

				MainCamera.transform.Rotate (xAngle, yAngle, zAngle, Space.World);
				break;
			}
		}
	}


	



}