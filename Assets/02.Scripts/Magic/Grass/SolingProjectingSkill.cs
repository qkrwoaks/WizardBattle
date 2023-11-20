using Photon.Pun;
using UnityEngine;

public class SolingProjectingSkill : MonoBehaviour
{
    private PhotonView PV;
    public float _speed;
    public float _damage;
    public float _duration;

    private float dis;
    private float waitTime;
    public Transform targetTr;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        FindTarget();

        dis = Vector3.Distance(transform.position, targetTr.position);

        //����ü ���� ó���� �а� �־����� ȸ���ϱ� ���ؼ�
        //����ü�� ȸ���� ĳ������ġ���� ����ü�� ��ġ�� �������� �����ϴ�
        transform.rotation = Quaternion.LookRotation(transform.position - targetTr.position);

        Destroy(gameObject, 6f);
    }

    private void FindTarget()
    {
        targetTr = PV.IsMine ? GameSystem.Instance.enemy.transform : GameSystem.Instance.player.transform;

        if (targetTr == null)
        {
            Debug.LogError("Not Found Target!!");
        }
    }

    void Update()
    {
        InductionMove();
    }

    void InductionMove()
    {
        if (targetTr == null) return;

        waitTime += Time.deltaTime;
        //1.5�� ���� õõ�� forward �������� ����
        if (waitTime < 1.5f)
        {
            _speed = Time.deltaTime;
            transform.Translate(transform.forward * _speed, Space.World);
        }
        else
        {
            // 1.5�� ���� Ÿ�ٹ������� lerp��ġ�̵�

            _speed += Time.deltaTime;
            float t = _speed / dis;

            transform.position = Vector3.LerpUnclamped(transform.position, targetTr.position, t);
        }

        // �������Ӹ��� Ÿ�ٹ������� ��ź�� ������ �ٲ�
        //����ü ��ġ - ��ź��ġ = ����ü�� Ÿ�����׼��� ����
        Vector3 directionVec = targetTr.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSystem.Instance.enemy == other.gameObject &&
                    other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player) && PV.IsMine == true)
        {
            player.Hit(_damage);
            Destroy(gameObject);
        }
    }
}
