using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GrowHahaSkill : Skill
{
    public Transform target;                                // 탈겟.

    [SerializeField] private float growTime;                // 커지는 시간
    [SerializeField] private Vector3 maxGrowthScale;        // 최대 크기

    protected override void Start()
    {
        FindTarget();

        base.Start();
    }

    protected override void UseSkill()
    {
        StartCoroutine(StartSkill());
    }

    private void FindTarget()
    {
        target = PV.IsMine ? GameSystem.Instance.enemy.transform : GameSystem.Instance.player.transform;

        if (target == null)
        {
            Debug.LogError("Not Found Target!!");
        }
    }

    private IEnumerator StartSkill()
    {
        Vector3 defalutScale = Vector3.one;
        
        yield return StartCoroutine(ChangeScaleCor(defalutScale, maxGrowthScale));

        yield return new WaitForSeconds(_duration);

        yield return StartCoroutine(ChangeScaleCor(maxGrowthScale, defalutScale));
    }

    private IEnumerator ChangeScaleCor(Vector3 start, Vector3 end)
    {
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            // growTime으로 나누어서 growTime 시간 동안
            // percent 값이 0에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / growTime;
            
            PV.RPC("ScaleUpdate",RpcTarget.All, Vector3.Lerp(start, end, percent));

            yield return null;
        }

        PV.RPC("ScaleUpdate", RpcTarget.All, end);
    }

    [PunRPC]
    public void ScaleUpdate(Vector3 scale)
    {
        if (target != null)
        {
            target.localScale = scale;
        }
    }
}