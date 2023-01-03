using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    private bool _active = true;

    [SerializeField] private GameObject cameraMan;
    [SerializeField] private GameObject mentor;

    private void Update()
    {
        if (cameraMan.layer == LayerMask.NameToLayer("Dead"))
        {
            mentor.GetComponent<MentorAI>().targetFollowEnabled = true;
            _active = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D playerCollided)
    {
        if (!_active)
            return;

        if (playerCollided.TryGetComponent<Player.PlayerController>(out var player))
        {
            AlarmManager.AlarmEnemiesByLoudSound(transform, 200);
            _active = false;
        }
    }
}
