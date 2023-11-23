using System.Collections;
using UnityEngine;
using Photon.Pun;

public class FieldSkill : Skill
{
    #region Variable(����)
    [SerializeField] private GameObject magicEffect;         // ���� ����Ʈ�� ������ �ִ� ���� ������Ʈ
    [SerializeField] private GameObject expectedRangePrefab; // ���� ��ų ������ �����ִ� ���� ������Ʈ ������

    [SerializeField] private float offset;                   // ������

    [SerializeField] private float skillRange = 50;          // �ִ� ��ų ��� ����

    private GameObject expectedRange;                        // ���� ���� ������Ʈ
    private Transform weaponTr;

    private RaycastHit rayHit;                               // ������ ����� �� ����Ǵ� ����ü
    private Ray ray;                                         // ���� ������ ����

    private Transform ERTransform => expectedRange.transform;// ������ �ڵ�

    private GameObject magicObject;
    #endregion

    #region Function(�Լ�)

    private void Awake()
    {
        if (PV == null)
        {
            PV = GetComponent<PhotonView>();
        }
    }

    protected override void Start()
    {
        if (PV.IsMine == false)
        {
            return;
        }

        weaponTr = GameSystem.Instance.plyerWeaponTr; // �÷��̾� ī�޶� �ʱ�ȭ
        base.Start();               // �θ� Ŭ������ Start ȣ��
    }

    /// <summary>
    /// ��ų�� ����ϴ� �Լ�
    /// </summary>
    protected override void UseSkill()
    {
        expectedRange = Instantiate(expectedRangePrefab, transform.position, transform.rotation); // ���� ���� ������Ʈ ����
        ray = new Ray();                                                                          // ���ο� ���� ����
        StartCoroutine(ShowExpectedLocation());                                                   // ���� ��ų ��� ������ �����ִ� �Լ� ����
    }

    /// <summary>
    /// ���� ��ų ��� ������ �����ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowExpectedLocation()
    {
        while (Input.GetMouseButton(0) == false)                                   // ���콺 ���� ��ư�� ������ �ʾ��� �� (���� ���� ����)
        {
            ray.origin = weaponTr.transform.position;                          // ���� ��ġ �ʱ�ȭ
            ray.direction = weaponTr.transform.forward;                        // ���� ���� �ʱ�ȭ
            if (Physics.Raycast(ray.origin, ray.direction, out rayHit, skillRange)) // ��� ��ü�� ������ ����� ��
            {
                ERTransform.position = rayHit.point + Vector3.up * 0.5f;                   // ���� ��ų ���� ������Ʈ ��ġ �ʱ�ȭ
            }
            yield return new WaitForEndOfFrame();                                  // ������ ���� ���
        }
        StartCoroutine(StartSkill());                                              // ��ų�� ���� �Լ�
        yield return null;                                                         // �ڷ�ƾ ����
    }

    /// <summary>
    /// ��ų�� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartSkill()
    {
        // ���� ���� ��ȯ
        magicObject = PhotonNetwork.Instantiate(magicEffect.name,
            new Vector3(ERTransform.position.x, ERTransform.position.y + offset, ERTransform.position.z),
            Quaternion.Euler(new Vector3(-90f, 0, 0)));

        magicObject.GetComponent<FieldSkillEffect>().skillATK = _ATK * GameSystem.Instance.playerManager.ATK;

        Destroy(expectedRange);                                // ���� ��ų ���� ������Ʈ ����

        yield return new WaitForSeconds(_duration - 1f);   // ��Ÿ�� -1 �� ���
        magicObject.GetComponent<ParticleSystem>().Stop(); // ��ƼŬ ����

        DestroyEffect();
        yield return new WaitForSeconds(1f);               // 1�� ���

        DestroyObject();

        StopCoroutine(StartSkill());
    }

    private void DestroyEffect()
    {
        if (magicObject != null)
        {
            PhotonNetwork.Destroy(magicObject);
        }
    }

#if UNITY_EDITOR // ����Ƽ �������� ��

    /// <summary>
    /// ����� �׸��� ȣ��Ǵ� �Լ�
    /// </summary>
    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction * skillRange, Color.red); // ������ ���� ����Ƽ �����Ϳ��� ���̰� ��
    }
#endif

    #endregion
}