using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnInteract : MonoBehaviour
{
    private bool _isActive;
    private bool _canInteract;

    [SerializeField] private GameObject _visualCuePreFab;
    private GameObject _visualCue;
    private Quaternion _visualCueRotation;

    private void Start()
    {
        float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
        float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;

        _visualCue = Instantiate(_visualCuePreFab, transform, false);

        _visualCue.SetActive(false);

        _visualCue.transform.position = new Vector2(transform.GetComponent<Collider2D>().bounds.center.x, transform.position.y + heightOfObject + 0.15f);
        _visualCueRotation = _visualCue.transform.rotation;

        _isActive = true;
        _canInteract = false;
    }

    private void Update()
    {
        if (_canInteract && _isActive)
            if (Input.GetKeyDown(KeyCode.E))
            {
                _visualCue.SetActive(false);
                _isActive = false;
                AlarmManager.AlarmEnemiesByQuietSound(transform, 20);                
            }
    }

    private void OnTriggerStay2D(Collider2D playerCollided)
    {
        if (!_isActive)
            return;

        if (playerCollided.TryGetComponent<Player.PlayerController>(out var player))
        {
            float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
            float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;
            _visualCue.transform.rotation = _visualCueRotation;
            _visualCue.transform.position = new Vector2(transform.position.x + widthOfObject / 2, transform.position.y + heightOfObject + 0.15f);
            _visualCue.SetActive(true);
            _canInteract = true;

        }
    }

    public void OnTriggerExit2D(Collider2D playerCollided)
    {
        _visualCue.SetActive(false);
        _canInteract = false;
    }
}