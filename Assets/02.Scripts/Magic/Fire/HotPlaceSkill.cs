using UnityEngine;

public class HotPlaceSkill : FieldSkillEffect
{
    private float time = 0f;

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;

        if (time > 0.5f)
        {
            if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                player.Hit(skillATK * GameSystem.Instance.playerManager.ATK);
            }
            time = 0;
        }
    }
}
