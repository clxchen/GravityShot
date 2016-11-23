using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class itemReceiver : NetworkBehaviour {

    fireControl fireControl;
    public AudioSource receiveAudio;

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
                if (receiveAudio)
                    receiveAudio.Play();
            }
            else
            {
                Debug.Log("can't find fire control in this player");
            }
        }
    }


}
