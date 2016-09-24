using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShooting :  NetworkBehaviour{

	private int damage = 25;
	private float range = 200; // Raycastの距離
	// FirstPersonCharacterを指定
	[SerializeField]
	private Transform cameraTransform;

	private RaycastHit hit;

	void Update()
	{
		CheckIfShooting();
	}

	void CheckIfShooting()
	{
		if (!isLocalPlayer) return;
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Shooting();
		}
	}

	void Shooting()
	{
		// カメラの前方にRaycastを飛ばす
		// TransformPoint: 指定した分だけ座標をずらす
		if (Physics.Raycast(cameraTransform.TransformPoint(0,0, 0.5f), cameraTransform.forward, out hit, range))
		{
			Debug.Log("hit:" + hit.transform.tag);
			// RaycastとPlayerが衝突したとき
			if (hit.transform.tag == "Player")
			{
				// 名前を取得
				string uIdentity = hit.transform.name;
				// 名前とダメージ量を引数にメソッド実行
				CmdTellServerWhoWasShow(uIdentity, damage);
			}
		}
	}

	[Command]
	void CmdTellServerWhoWasShow(string uniqueID, int dmg)
	{
		// 敵プレイヤーの名前でGameObjectを取得
		GameObject go = GameObject.Find(uniqueID);
		// ダメージを与える
		PlayerHealth  playerHealth = go.GetComponent<PlayerHealth>();
		if (playerHealth == null)
		{
			Debug.LogError("playerHealth is Null!");
			return;
		}
		playerHealth.DeductHealth(dmg);

	}
}
