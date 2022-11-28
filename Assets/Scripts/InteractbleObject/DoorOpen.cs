using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapDoorOpened;
    [SerializeField] private Tilemap tilemapDoorClosed;

    private void OnTriggerStay2D(Collider2D mortalCollided)
    {
        if (mortalCollided.TryGetComponent<Player.PlayerController>(out var player) || mortalCollided.TryGetComponent<EnemyAI>(out var enemy))
        {
            tilemapDoorClosed.gameObject.SetActive(false);
            tilemapDoorOpened.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D playerCollided)
    {
        tilemapDoorClosed.gameObject.SetActive(true);
        tilemapDoorOpened.gameObject.SetActive(false);
    }
}
