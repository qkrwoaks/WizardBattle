using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EarthquakeSkill : Skill
{
    [SerializeField] private Camera playerCamera;

    protected override void Start()
    {
        playerCamera = PV.IsMine == false ? Camera.main : null;

        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartEarthquake());
    }

    private IEnumerator StartEarthquake()
    {
        float timer = 0;

        while (timer <= _duration)
        {
            timer += 0.5f;
            PV.RPC("ShakeCamera", RpcTarget.All, Quaternion.Euler(new Vector3(0, 0, Random.Range(-1, 1) * 5)));
            yield return new WaitForSeconds(0.5f);
        }

        PV.RPC("ShakeCamera", RpcTarget.All, Quaternion.Euler(0,0,0));
    }

    [PunRPC]
    private void ShakeCamera(Quaternion quaternion)
    {
        if (playerCamera != null)
        {
            playerCamera.transform.rotation = quaternion;
        }
    }
}
