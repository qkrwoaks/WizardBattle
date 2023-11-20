using UnityEngine;

public class FieldOfGrassSkill : FieldSkillEffect
{
    private float defalutAttackSpeed;
    PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            // �÷��̾� ���ݼӵ� ����ü �ӵ� ����
            playerManager = player;
            defalutAttackSpeed = playerManager.attackSpeed;
            playerManager.attackSpeed -= 0.2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            // �÷��̾� ���ݼӵ� ����ü �ӵ� �ٽ� ����
            playerManager.attackSpeed = defalutAttackSpeed;
        }
    }

    private void OnDestroy()
    {
        if (gameObject && defalutAttackSpeed != 0)
            playerManager.attackSpeed = defalutAttackSpeed;
    }
}
