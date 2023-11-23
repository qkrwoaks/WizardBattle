using System.Collections;
using UnityEngine;

public class FireLaserSkill : Skill
{
    [SerializeField]
    private Transform weaponTr;

    private Vector3 offset = Vector3.zero;

    private bool isStay;
    private PlayerManager _enemy;

    protected override void Start()
    {
        weaponTr = PV.IsMine ? GameSystem.Instance.plyerWeaponTr : GameSystem.Instance.enemyWeaponTr;

        base.Start();
    }

    protected override void UseSkill()
    {
        StartSkill();
    }

    private void StartSkill()
    {
        StartCoroutine(StartSkillCor());
    }

    private IEnumerator StartSkillCor()
    {
        float current = 0;

        while (current < _duration)                                   // ���콺 ���� ��ư�� ������ �ʾ��� �� (���� ���� ����)
        {
            current += Time.deltaTime;

            transform.position = weaponTr.position + offset;
            transform.rotation = weaponTr.transform.rotation;

            yield return new WaitForEndOfFrame();                          // ������ ���� ���
        }

        yield return null;                                                  // �ڷ�ƾ ����

        DestroyObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSystem.Instance.enemy == other.gameObject &&
            other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager enemy) && PV.IsMine == true)
        {
            isStay = true;
            _enemy = enemy;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isStay == true)
        {
            _enemy.Hit(_ATK * GameSystem.Instance.playerManager.ATK);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameSystem.Instance.enemy == other.gameObject &&
            other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager enemy) && PV.IsMine == true)
        {
            isStay = false;
        }
    }
}
