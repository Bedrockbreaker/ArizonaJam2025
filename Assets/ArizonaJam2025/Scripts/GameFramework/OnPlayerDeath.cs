using UnityEngine;

public class OnPlayerDeath : MonoBehaviour
{
    private RespawnCheckpoint currentCheckpoint;

    public void Start()
    {
        KillPlayer();
    }

    public void KillPlayer()
    {
        // trigger the Scene to change to the game over screen
        GameManager.Instance.LoadScene("S_GameOver");
    }
}
