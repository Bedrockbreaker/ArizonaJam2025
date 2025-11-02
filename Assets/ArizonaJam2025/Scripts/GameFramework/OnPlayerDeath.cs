using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayerDeath : MonoBehaviour
{
    private RespawnCheckpoint currentCheckpoint;

    public void KillPlayer()
    {
        // trigger the Scene to change to the game over screen
        GameManager.Instance.LoadScene("S_GameOver");
    }
}
