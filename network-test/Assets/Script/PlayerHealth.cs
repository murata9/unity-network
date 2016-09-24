using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour {

	[SyncVar(hook = "OnHealthChanged")]
	private int health = 100;

	private Text healthText;

	void Start()
	{
		// Textオブジェクトをキャッシュ
		healthText = GameObject.Find("Health Text").GetComponent<Text>();
		if (healthText == null) Debug.LogError("HealthText is Null!");
		SetHealthText();
	}

	void SetHealthText()
	{
		if (!isLocalPlayer) return;
		healthText.text = "Health " + health.ToString();
	}

	// ダメージを受けた時のメソッド
	public void DeductHealth(int dmg)
	{
		health -= dmg;
	}

	// hookメソッド
	void OnHealthChanged(int hlth)
	{
		// 全クライアントへ変更結果を送信
		health = hlth;
		// HPの表示を更新
		SetHealthText();
	}
}
