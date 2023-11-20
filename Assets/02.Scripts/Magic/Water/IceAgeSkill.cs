using UnityEngine;

public class IceAgeSkill : FieldSkillEffect
{
    float defalutSpeed;
    PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            playerManager = player;
            defalutSpeed = playerManager.speed;
            playerManager.speed *= 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
        {
            playerManager.speed = defalutSpeed;
        }
    }

    private void OnDestroy()
    {
        if (gameObject && defalutSpeed != 0)
            playerManager.speed = defalutSpeed;
    }
}