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

    [SerializeField] private float delay;

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
