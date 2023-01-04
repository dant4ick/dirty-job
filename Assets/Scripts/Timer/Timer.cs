using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject dialogueAboveEnemy;

    [SerializeField] private List<GameObject> enemiesToGo;
    [SerializeField] private List<Transform> positionsToGo;

    public float timeLeft;
    public bool timerOn = false;

    private TextMeshProUGUI _text;

    [SerializeField] AudioClip clip;

    private void Start()
    {
        timerOn = true;
        _text = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0f;
                UpdateTimer(timeLeft);

                timerOn = false;

                foreach (GameObject enemy in enemies)
                {
                    GameObject dialogueCanvas = Instantiate(dialogueAboveEnemy, enemy.transform);
                    Transform dialogueToSay = dialogueCanvas.transform.GetChild(1);
                    dialogueToSay.GetComponent<DialogueAboveEnemy>().mainCamera = Player.PlayerController.Instance.mainCamera;
                    dialogueToSay.GetComponent<DialogueAboveEnemy>().inputPhrase.Add("Pizza time!");
                }

                for (int enemy = 0; enemy < enemiesToGo.Count; enemy++)
                {
                    enemiesToGo[enemy].GetComponent<AlarmManager>().GoToSound(positionsToGo[enemy]);
                }

                SoundManager.PlayMicrowaveSound(clip);

                Destroy(transform.parent.parent.gameObject);
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        //currentTime += 1;

        float seconds = Mathf.FloorToInt(currentTime % 60);
        float miliseconds = Mathf.FloorToInt((currentTime - Mathf.FloorToInt(currentTime)) * 100);

        _text.text = string.Format("{0:00}.{1:00}", seconds, miliseconds);
    }
}
