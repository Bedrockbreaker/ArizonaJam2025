using Unity.VisualScripting;
using UnityEngine;

public class RespawnCheckpoint : Trigger
{
    public void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // set this to the current checkpoint
        SetCheckpoint();
    }

    // TODO
    public void SetCheckpoint()
    {

    }
}
