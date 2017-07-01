using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUX.Collections;

public class PlayerList : ObjectCollection
{
    public static PlayerList instance = null;

    public void OnEnable()
    {
        instance = this;
    }

    public void AddPlayer(HoloLensLobbyPlayer player)
    {
        // Need to clone the lobby player prefab here at this point..
        var cn = new ObjectCollection.CollectionNode();
        cn.transform = player.transform;
        player.transform.parent = this.transform;
        NodeList.Add(cn);
        UpdateCollection();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
