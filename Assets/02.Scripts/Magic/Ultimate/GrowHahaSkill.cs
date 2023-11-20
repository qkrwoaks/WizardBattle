using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GrowHahaSkill : Skill
{
    public Transform target;                                // Ż��.

    [SerializeField] private float growTime;                // Ŀ���� �ð�
    [SerializeField] private Vector3 maxGrowthScale;        // �ִ� ũ��

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
            // growTime���� ����� growTime �ð� ����
            // percent ���� 0���� 1�� �����ϵ��� ��
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