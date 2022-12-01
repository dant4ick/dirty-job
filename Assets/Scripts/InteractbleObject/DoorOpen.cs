using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapDoorOpened;
    [SerializeField] private Tilemap tilemapDoorClosed;

    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool _doorOpened = false;

    private void OnTriggerStay2D(Collider2D mortalCollided)
    {
        if (_doorOpened)
            return;

        if (mortalCollided.TryGetComponent<Player.PlayerController>(out var player) || mortalCollided.TryGetComponent<EnemyAI>(out var enemy))
        {
            SoundManager.PlayEnvironmentSound(openSound);
            _doorOpened = true;

            tilemapDoorClosed.gameObject.SetActive(false);
            tilemapDoorOpened.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D playerCollided)
    {
        SoundManager.PlayEnvironmentSound(closeSound);

        tilemapDoorClosed.gameObject.SetActive(true);
        tilemapDoorOpened.gameObject.SetActive(false);

        _doorOpened = false;
    }
}
