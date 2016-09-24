using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings(channel=0, sendInterval=0.0033f)]
public class PlayerSyncPosition : NetworkBehaviour
{
	[SyncVar]
	private Vector3 syncPos; // ホストから全クライアントに送られる
	[SerializeField]
	Transform myTransform;  // プレイヤーの現在位置
							// Lerp: 2ベクトル間を補完する
	[SerializeField]
	float lerpRate = 15;

	// 前フレームの最終地点
	private Vector3 lastPos;
	// threshold: しきい値、境目となる値のこと
	// 0.5unitを越えなければ移動していないこととする
	private float threshold = 0.5f;

	// UI
	private NetworkClient nClient;
	private int latency; // 遅延時間
	private Text latencyText; // 遅延時間表示用テキスト
	
	void Start()
	{
		// NetworkClientとTextをキャッシュする
		nClient = GameObject.Find("NetWorkManager").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
	}

	void Update()
	{
		LerpPosition();
		ShowLatency();
	}

	void FixedUpdate()
	{
		TransmitPosition();
	}

	// ポジション補間
	void LerpPosition()
	{
		// 補間対象は相手プレイヤーのみ
		if (!this.isLocalPlayer)
		{
			// Lerp(from, to, Rate) form～toのベクトルを補完する
			myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	// クライアントからホストへ、Position情報を送る
	[Command]
	void CmdProvidePositionToServer(Vector3 pos)
	{
		// サーバー側が受け取る値
		syncPos = pos;
		// ホストなら毎フレーム呼ばれていた
		// Debug.Log("CmdProvidePositionToServer");
	}

	// クライアントのみ実行される
	[ClientCallback]
	// 位置情報を送るメソッド
	void TransmitPosition()
	{
		if (!isLocalPlayer) return;
		if (Vector3.Distance(myTransform.position, lastPos) > threshold)
		{
			CmdProvidePositionToServer(myTransform.position);
			lastPos = myTransform.position;
		}
	}

	void ShowLatency()
	{
		if (!isLocalPlayer) return;
		// latencyを取得
		latency = nClient.GetRTT();
		// latencyを表示
		latencyText.text = latency.ToString();
	}
}