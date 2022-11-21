using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueAboveEnemy : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private TMP_Text _dialogueText;

    [SerializeField] private string _input;

    [SerializeField] private float _delay;

    private bool _check = true;

    private void Update()
    {
        //Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, _mainCamera.transform.position.z);

        if (_check)
        {
            Vector3 screenHeight = new Vector3(Screen.width / 2, Screen.height, _mainCamera.transform.position.z);
            Vector3 screenWidth = new Vector3(Screen.width, Screen.height / 2, _mainCamera.transform.position.z);
            Vector3 goscreen = _mainCamera.WorldToScreenPoint(transform.position);

            float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f, 0f));
            float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));

            if (distX < screenWidth.x / 2 && distY < screenHeight.y / 2)
                ShowDialogue();
        }
    }

    private IEnumerator WriteText(string input, TMP_Text text, float delay)
    {
        for (int character = 0; character < input.Length; character++)
        {
            text.text += input[character];
            yield return new WaitForSeconds(delay);
        }
    }

    private void ShowDialogue()
    {
        _check = false;
        StartCoroutine(WriteText(_input, _dialogueText, _delay));
    }
}
