using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DisableNetworkUnlocalplayerBehaviour : NetworkBehaviour {
    [SerializeField]
    Behaviour[] behaviours;

	void Start () {
        Debug.Log("isLocalPlayer?" + this.isLocalPlayer);

        if (!this.isLocalPlayer)
        {
            foreach (var b in behaviours)
            {
                b.enabled = false;
            }
        }
	}
	
    void OnApplicationFocus(bool focusState)
    {
        //if (this.isLocalPlayer)
        //{
        //    foreach (var b in behaviours)
        //    {
        //        b.enabled = focusState;
        //    }
        //}
    }
}
