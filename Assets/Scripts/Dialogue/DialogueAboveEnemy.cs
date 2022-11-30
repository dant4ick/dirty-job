using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueAboveEnemy : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float delay;

    public Camera mainCamera;
    public List<string> inputPhrase;

    private bool _check = true;

    private void Update()
    {
        if (transform.lossyScale.x == -0.5)
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else if (transform.lossyScale.x == 0.5)
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);

        if (transform.lossyScale.x == -1)
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else if (transform.lossyScale.x == 1)
            transform.parent.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);

        if (_check)
        {
            Vector3 screenHeight = new Vector3(Screen.width / 2, Screen.height, mainCamera.transform.position.z);
            Vector3 screenWidth = new Vector3(Screen.width, Screen.height / 2, mainCamera.transform.position.z);
            Vector3 goscreen = mainCamera.WorldToScreenPoint(transform.position);

            float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f, 0f));
            float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));

            if (distX < screenWidth.x / 2 && distY < screenHeight.y / 2)
                ShowDialogue();
        }
    }

    private IEnumerator WriteText(List<string> inputPhrase, TMP_Text text, float delay)
    {
        foreach (string phrase in inputPhrase)
        {
            for (int character = 0; character < phrase.Length; character++)
            {
                text.text += phrase[character];
                text.isRightToLeftText = false;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1.5f);
            text.text = null;
        }
    }

    private void ShowDialogue()
    {
        _check = false;
        StartCoroutine(WriteText(inputPhrase, dialogueText, delay));
    }
}
