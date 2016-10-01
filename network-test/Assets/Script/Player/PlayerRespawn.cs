using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRespawn : NetworkBehaviour
{
	private PlayerHealth playerHealthScript;
	private PlayerEnable playerEnableScript;

	void Start()
	{
		playerEnableScript = GetComponent<PlayerEnable>();
		if (GameManagerRefarences.NullCheck(playerEnableScript, "playerEnableScript")) return;
		playerHealthScript = GetComponent<PlayerHealth>();
		if (GameManagerRefarences.NullCheck(playerHealthScript, "playerHealthScript")) return;
		playerHealthScript.EventRespawn += EnablePlayer;
		SetRespawnButton();
	}

	void SetRespawnButton()
	{
		if (isLocalPlayer)
		{
			// GamaManagerを経由してRespawnButtonを取得
			GameObject respawnButton = GameManagerRefarences.instance.respawnButton;
			respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
			// ボタンを消す
			respawnButton.SetActive(false);
		}
	}

	// メモリーリークした時用の、安全のためのメソッド　そんなに重要じゃない
	// OnDisable: 無効になったときに呼ばれる
	void OnDisable()
	{
		// EventからDisablePlayerメソッドを削除
		playerHealthScript.EventRespawn -= EnablePlayer;
	}

	void EnablePlayer()
	{
		playerEnableScript.SetEnablePlayer(true);
	}

	// Respawnボタンを押したときに実行
	void CommenceRespawn()
	{
		CmdRespawnOnServer();
	}

	[Command]
	void CmdRespawnOnServer()
	{
		playerHealthScript.ResetHealth();
	}
}
