using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ZombieTarget : NetworkBehaviour {
	private NavMeshAgent agent;
	private Transform myTransform;
	private Transform targetTransform;
	// ゾンビが探知するレイヤー
	private LayerMask raycastLayer;
	// ゾンビがプレイヤーを探知する半径
	private float radius = 100f;

	[SerializeField]
	private float interval = 0.2f;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		GameManagerRefarences.NullCheck(agent, "agent");
		myTransform = this.transform;
		raycastLayer = 1 << LayerMask.NameToLayer("Player");
		// コルーチンの実行
		if (isServer)
		{
			StartCoroutine(DoCheck());
		}
	}

	IEnumerator DoCheck()
	{
		while(true)
		{
			SearchForTarger();
			MoveForTaget();
			yield return new WaitForSeconds(interval);
		}
	}

	void SearchForTarger()
	{
		// サーバーじゃなければメソッド終了
		if (!isServer)
		{
			return;
		}
		// プレイヤーをまだ取得していないとき
		if (targetTransform == null)
		{
			// Physics.OverlapSphere: ある地点を中心に球を作り、衝突したオブジェクトを取得する
			// 第一引数: 中心点,第二引数: 半径, 第三引数: 対象レイヤー
			Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);
			if (hitColliders.Length > 0)
			{
				// プレイヤーを発見した
				int randomInt = Random.Range(0, hitColliders.Length); // 複数発見した時はランダムに取得
				targetTransform = hitColliders[randomInt].transform;
			}
		}
		if (targetTransform != null)
		{
			BoxCollider box = null;
			for (int i = 0; i < 10; i++)
			{
				box = targetTransform.GetComponent<BoxCollider>();
				if (GameManagerRefarences.NullCheck(box, "targetBoxCollinder"))
				{
					Debug.Log("TargetName:" + targetTransform.gameObject.name);
					return;
				}

			}
			if (box.enabled == false)
			{
				targetTransform = null;
			}
		}
	}

	void MoveForTaget()
	{
		if (!isServer) return;
		// プレイヤーオブジェクトを取得済みのとき
		if (targetTransform != null)
		{
			SetNavDestination(targetTransform);
		}
	}

	void SetNavDestination(Transform dest)
	{
		agent.SetDestination(dest.position);
	}
}
