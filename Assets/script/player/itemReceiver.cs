using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class itemReceiver : NetworkBehaviour {

    fireControl fireControl;

    void Start()
    {
        fireControl = GetComponent<fireControl>();

    }



    [ClientRpc]
    public void RpcReceiveItem(BulletType bulletType)
    {
        if (isLocalPlayer)
        {
            if (fireControl)
            {
                fireControl.getBullet(bulletType);
            }
            else
            {
                Debug.Log("can't find fire control in this player");
            }
        }
    }


}
