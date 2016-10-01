using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerDeath : NetworkBehaviour {
	private PlayerHealth healthScript;
	private Image crossHairImage; // 照準のImage

	readonly string crossHairImageObjectName = "crosshair Image";
	// Use this for initialization
	void Start()
	{
		// キャッシュしておく
		GameObject crossHairImageObject = GameObject.Find(crossHairImageObjectName);
		if (crossHairImageObject == null)
		{
			Debug.LogError("crossHairImageObject is Null!");
			return;
		}
		crossHairImage = crossHairImageObject.GetComponent<Image>();
		if (crossHairImage == null)
		{
			Debug.LogError("crossHairImage is Null!");
			return;
		}
		healthScript = GetComponent<PlayerHealth>();
		if (healthScript == null)
		{
			Debug.LogError("healthScript is Null!");
			return;
		}
		healthScript.EventDie += DisablePlayer;
	}

	// メモリーリークした時用の、安全のためのメソッド　そんなに重要じゃない
	// OnDisable: 無効になったときに呼ばれる
	void OnDisable()
	{
		// EventからDisablePlayerメソッドを削除
		healthScript.EventDie -= DisablePlayer;
	}

	// Eventで登録されるメソッド CheckConditionメソッド内で使われる
	// 各コンポーネントを非アクティブ状態にする
	void DisablePlayer()
	{
		Debug.Log("DisablePlayer Called");
		GetComponent<CharacterController>().enabled = false;
		GetComponent<PlayerShooting>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;

		// 子オブジェクトのRendererを全て格納
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		// 格納したRendererを全て格納
		foreach (Renderer ren in renderers)
		{
			ren.enabled = false;
		}
		// isDeadをtrueにすることでCheckConditionのif文内に入らないようにする
		healthScript.isDead = true;
		if(isLocalPlayer)
		{
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
			crossHairImage.enabled = false;
			// Respawn Button
			// 次回以降でPlayerを復活させるためのボタンを生成する個所
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
