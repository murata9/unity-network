using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManagerRefarences : MonoBehaviour {
	[SerializeField]
	public Image crosshairImage;
	[SerializeField]
	public GameObject respawnButton;

	public static GameManagerRefarences instance;

	public static bool NullCheck(Object obj, string name)
	{
		if (obj == null)
		{
			Debug.LogError(name + " is Null!");
			return true;
		}
		return false;
	}
	void Start()
	{
		NullCheck(crosshairImage, "crosshairImage");
		NullCheck(respawnButton, "respawnButton");
		instance = this;
	}
}
