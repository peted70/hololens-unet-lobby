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

        // try not reparenting here as it causes a problem where when you switch scenes to the 
        // game scene these get destroyed and recreated in an unexpected location in the hierarchy
        //
        cn.transform = player.transform;
        //player.transform.parent = this.transform;
        NodeList.Add(cn);
        UpdateCollection();
    }
}
