using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerID : NetworkBehaviour {

	// SyncVar: [Command]で変更後、全クライアントに変更結果を送信
	[SyncVar]
	private string playerUniqueIdentity;

	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer()
	{
		GetNetIdentity();
		SetIdentity();
	}
	
	void Awake()
	{
		// 自分の名前を取得するときに使う
		myTransform = this.gameObject.transform;
	}

	void Update()
	{
		// 自分以外のプレイヤーの名前を設定する
		if (string.IsNullOrEmpty(myTransform.name) || myTransform.name == "Player(Clone)")
		{
			SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity()
	{
		// NetworkIdentityのNetID取得
		playerNetID = GetComponent<NetworkIdentity>().netId;
		// 名前を付けるメソッド実行
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}

	void SetIdentity()
	{
		// 自分以外のPlayerオブジェクトの場合
		if (!isLocalPlayer) {
			// 今ついている名前のまま
			myTransform.name = playerUniqueIdentity;
		}
		else
		{
			// 自分自身の場合、MakeUniqueIdentityで名前を取得
			myTransform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity()
	{
		// Player + NetIDで名前を付ける
		string uniqueName = "Player " + playerNetID.ToString();
		return uniqueName;
	}

	// Command: SyncVar変数を変更し、変更結果を全クライアントに送る
	[Command]
	void CmdTellServerMyIdentity(string name)
	{
		playerUniqueIdentity = name;
	}
}
