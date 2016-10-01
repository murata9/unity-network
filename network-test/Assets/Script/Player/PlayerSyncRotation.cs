using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[NetworkSettings(channel = 0, sendInterval = 0.0033f)]
public class PlayerSyncRotation : NetworkBehaviour {

	// SyncVar: ホストサーバーからクライアントに送られる
	// プレイヤーの角度
	[SyncVar]
	private Quaternion syncPlayerRotation;
	// FirstPersonCharacterのカメラ角度
	[SyncVar]
	private Quaternion syncCameraRotation;

	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private float lerpRate = 15;

	// 前フレームの最終角度
	private Quaternion lastPlayerRot;
	private Quaternion lastCamRot;
	// しきい値は5度
	private float threshold = 5;

	void Update()
	{
		// 現在角度と取得した角度を補間する
		LerpRotations();
	}

	void FixedUpdate()
	{
		// クライアント側のPlayerの角度を取得
		TransmitRotations();
	}

	// 角度を補間するメソッド
	void LerpRotations()
	{
		// 自プレイヤー以外のPlayerの時
		if (!isLocalPlayer)
		{
			// プレイヤーの角度とカメラの角度を補間
			playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation,
				syncPlayerRotation, Time.deltaTime * lerpRate);
		}
	}

	// クライアントからホストに送られる
	[Command]
	void CmdProvideRotationsToServer(Quaternion playerRot, Quaternion camRot)
	{
		syncPlayerRotation = playerRot;
		syncCameraRotation = camRot;
	}

	// クライアント側だけが実行できるメソッド
	[Client]
	void TransmitRotations()
	{
		if (!isLocalPlayer) return;
		if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold
			|| Quaternion.Angle(cameraTransform.rotation, lastCamRot) > threshold)
		{
			CmdProvideRotationsToServer(playerTransform.rotation, cameraTransform.rotation);
			// 最終角度を保存
			lastPlayerRot = playerTransform.rotation;
			lastCamRot = cameraTransform.rotation;
		}
	}
}
