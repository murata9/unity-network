using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManagerZombieSpawner : NetworkBehaviour {
	[SerializeField]
	GameObject zombiePrefab;
	[SerializeField]
	GameObject zombieSpawn;

	private int counter;
	private int numberOfZombie = 10;

	public override void OnStartServer()
	{
		for(int i = 0; i < numberOfZombie; i++)
		{
			SpawnZombies();
		}
	}

	void SpawnZombies()
	{
		// counter

		GameObject zombie = GameObject.Instantiate(zombiePrefab, zombieSpawn.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(zombie);
	}
}
