using UnityEngine;

public class FieldOfGrassSkill : FieldSkillEffect
{
    private float defalutAttackSpeed;
    PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            // 플레이어 공격속도 투사체 속도 증가
            playerManager = player;
            defalutAttackSpeed = playerManager.attackSpeed;
            playerManager.attackSpeed -= 0.2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            // 플레이어 공격속도 투사체 속도 다시 감소
            playerManager.attackSpeed = defalutAttackSpeed;
        }
    }

    private void OnDestroy()
    {
        if (gameObject && defalutAttackSpeed != 0)
            playerManager.attackSpeed = defalutAttackSpeed;
    }
}
