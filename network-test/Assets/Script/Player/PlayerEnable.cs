using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEnable : NetworkBehaviour
{
	public void SetEnablePlayer(bool enable)
	{
		GetComponent<CharacterController>().enabled = enable;
		GetComponent<PlayerShooting>().enabled = enable;
		GetComponent<BoxCollider>().enabled = enable;

		// 子オブジェクトのRendererを全て格納
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		// 格納したRendererを全て格納
		foreach (Renderer ren in renderers)
		{
			ren.enabled = enable;
		}
		if (isLocalPlayer)
		{
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
			// 照準
			GameManagerRefarences.instance.crosshairImage.enabled = enable;
			// Respawn Buttonの表示切り替え
			GameManagerRefarences.instance.respawnButton.SetActive(!enable);
		}
	}
}
