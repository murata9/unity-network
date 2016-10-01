using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerDeath : NetworkBehaviour {
	private PlayerHealth playerHealthScript;
	private PlayerEnable playerEnableScript;

	void Start()
	{
		playerEnableScript = GetComponent<PlayerEnable>();
		if (GameManagerRefarences.NullCheck(playerEnableScript, "playerEnableScript")) return;
		playerHealthScript = GetComponent<PlayerHealth>();
		if (GameManagerRefarences.NullCheck(playerHealthScript, "playerHealthScript")) return;
		playerHealthScript.EventDie += DisablePlayer;
	}

	// メモリーリークした時用の、安全のためのメソッド　そんなに重要じゃない
	// OnDisable: 無効になったときに呼ばれる
	void OnDisable()
	{
		// EventからDisablePlayerメソッドを削除
		playerHealthScript.EventDie -= DisablePlayer;
	}

	// Eventで登録されるメソッド CheckConditionメソッド内で使われる
	// 各コンポーネントを非アクティブ状態にする
	void DisablePlayer()
	{
		playerEnableScript.SetEnablePlayer(false);
		playerHealthScript.isDead = true;
	}
}
