using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ZombieID : NetworkBehaviour {
	[SyncVar]
	public string zombieID;
	private Transform myTransform;

	void Start()
	{
		myTransform = transform;
	}

	void Update()
	{
		SetIdentity();
	}

	void SetIdentity()
	{
		if (myTransform.name == "" || myTransform.name == "Zombie(Clone)")
		{
			// zombieIDは、GameManagerZombieSpawnerクラスにて設定される
			myTransform.name = zombieID;
		}
	}
}
