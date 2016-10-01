using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour {

	[SyncVar(hook = "OnHealthChanged")]
	private int health = 100;

	private Text healthText;
	private bool shouldDie = false;
	public bool isDead = false;
	// 死んだときに機能するDelegateとEvent
	// Event: メソッドを登録しておき、任意のタイミングで呼び出す
	public delegate void DieDelegate();
	public event DieDelegate EventDie;

	void Start()
	{
		// Textオブジェクトをキャッシュ
		healthText = GameObject.Find("Health Text").GetComponent<Text>();
		if (healthText == null) Debug.LogError("HealthText is Null!");
		SetHealthText();
	}

	void Update()
	{
		CheckCondition();
	}

	void CheckCondition()
	{
		// HPが0以下になったとき
		if (health <= 0 && !shouldDie && !isDead)
		{
			shouldDie = true;
		}

		if (health <= 0 && shouldDie)
		{
			// Eventが登録されているとき
			if (EventDie != null)
			{
				// Event実行
				EventDie();
			}
			shouldDie = false;
		}
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
