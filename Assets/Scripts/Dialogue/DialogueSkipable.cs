using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueSkipable : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image imageHolder;

    [SerializeField] private List<string> inputPhrase;
    [SerializeField] private List<Sprite> speakerImage;
    [SerializeField] private List<string> caracterTalking;

    [SerializeField] private float delay;

    private bool _skip = false;

    [SerializeField] private AudioClip clipForBlondie;
    [SerializeField] private AudioClip clipForMentor;

    private void Start()
    {
        ShowDialogue();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            _skip = true;
    }

    private IEnumerator WriteText()
    {
        for (int phrase = 0; phrase < inputPhrase.Count; phrase++)
        {
            imageHolder.sprite = speakerImage[phrase];

            for (int character = 0; character < inputPhrase[phrase].Length; character++)
            {
                if (_skip)
                    break;

                if (caracterTalking[phrase] == "Mentor")
                    SoundManager.PlayCharacterSound(clipForMentor);
                else if (caracterTalking[phrase] == "Blondie")
                    SoundManager.PlayCharacterSound(clipForBlondie);

                dialogueText.text += inputPhrase[phrase][character];
                yield return new WaitForSeconds(delay);
            }
            if (!_skip)
                yield return new WaitForSeconds(3f);

            _skip = false;
            dialogueText.text = null;
        }

        this.gameObject.SetActive(false);
        SoundManager.instance.MusicSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ShowDialogue()
    {
        StartCoroutine(WriteText());
    }
}
