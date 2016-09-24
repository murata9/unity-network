using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour
{
	[SyncVar]
	private Vector3 syncPos; // ホストから全クライアントに送られる
	[SerializeField]
	Transform myTransform;  // プレイヤーの現在位置
							// Lerp: 2ベクトル間を補完する
	[SerializeField]
	float lerpRate = 15;

	void FixedUpdate()
	{
		TransmitPosition();
		LerpPosition();
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
	}

	// クライアントのみ実行される
	[ClientCallback]
	// 位置情報を送るメソッド
	void TransmitPosition()
	{
		if (isLocalPlayer)
		{
			CmdProvidePositionToServer(myTransform.position);
		}
	}
}