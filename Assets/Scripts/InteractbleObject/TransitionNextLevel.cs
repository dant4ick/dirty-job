using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionNextLevel : MonoBehaviour
{
    [SerializeField] private GameObject visualCuePreFab;
    [SerializeField] private Object nextScene;
    private GameObject _visualCue;
    private Quaternion _visualCueRotation;

    private int nextLevelToLoad;

    private bool _canInteract;
    // Start is called before the first frame update
    void Start()
    {
        nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
        float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;

        _visualCue = Instantiate(visualCuePreFab, transform, false);

        _visualCue.SetActive(false);

        _visualCue.transform.position = new Vector2(transform.GetComponent<Collider2D>().bounds.center.x, transform.position.y + heightOfObject + 0.15f);
        _visualCueRotation = _visualCue.transform.rotation;

        _canInteract = false;
    }

    private void Update()
    {
        if (_canInteract)
            if (Input.GetKeyDown(KeyCode.E))
            {
                _visualCue.SetActive(false);
                PlayerInventoryManager.Instance.SaveInventoryAndEquipment();
                SceneManager.LoadScene(nextLevelToLoad);
            }
    }

    private void OnTriggerStay2D(Collider2D playerCollided)
    {
        if (playerCollided.TryGetComponent<Player.PlayerController>(out var player))
        {
            float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
            float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;
            _visualCue.transform.rotation = _visualCueRotation;
            _visualCue.transform.position = new Vector2(transform.position.x, transform.position.y + heightOfObject / 2 + 0.0625f);
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
