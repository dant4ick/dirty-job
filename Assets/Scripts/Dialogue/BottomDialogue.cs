using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image imageHolder;

    [SerializeField] private List<string> inputPhrase;
    [SerializeField] private List<Sprite> speakerImage;
    [SerializeField] private List<string> caracterTalking;

    [SerializeField] private float delay;

    [SerializeField] private GameObject boss;

    [SerializeField] private AudioClip clipForBlondie;
    [SerializeField] private AudioClip clipForMentor;
    [SerializeField] private AudioClip clipForBoss;

    private void Start()
    {
        ShowDialogue();        
    }

    private IEnumerator WriteText()
    {
        for (int phrase = 0; phrase < inputPhrase.Count; phrase++)
        {
            imageHolder.sprite = speakerImage[phrase];

            for (int character = 0; character < inputPhrase[phrase].Length; character++)
            {
                if (caracterTalking[phrase] == "Mentor")
                    SoundManager.PlayEnvironmentSound(clipForMentor);
                else if (caracterTalking[phrase] == "Blondie")
                    SoundManager.PlayEnvironmentSound(clipForBlondie);
                else if (caracterTalking[phrase] == "Boss")
                    SoundManager.PlayEnvironmentSound(clipForBoss);

                dialogueText.text += inputPhrase[phrase][character];
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = null;
        }

        this.gameObject.SetActive(false);


    }

    private void ShowDialogue()
    {
        StartCoroutine(WriteText());
    }
}
