using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        Debug.Log("PlayerObject::Start -- Spawning");
        CmdSpawnMyUnit();
    }

    public GameObject PlayerUnitPrefab;

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        NetworkServer.Spawn(go);
    }
}
