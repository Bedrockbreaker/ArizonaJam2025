using UnityEngine;

public class DeathTrigger : Trigger
{
    public void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        OnPlayerDeath playerDeathComp = other.gameObject.AddComponent<OnPlayerDeath>();
        playerDeathComp.KillPlayer();
    }
}
