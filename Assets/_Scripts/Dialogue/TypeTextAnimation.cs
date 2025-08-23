using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class TypeTextAnimation : MonoBehaviour
{
    public Action TypeFinished;
    public float typeDelay = 0.05f;
    public TextMeshProUGUI textObj;
    public TextMeshProUGUI nameObj;
    public string text;
    public string speaker;

    Coroutine coroutine;

    public void StartTyping()
    {
        coroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textObj.maxVisibleCharacters = 0;
        nameObj.text = speaker;
        textObj.text = text;
        for (int i = 0; i <= textObj.text.Length; i++)
        {
            textObj.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeDelay);
        }

        TypeFinished?.Invoke();
    }

    public void Skip()
    {
        StopCoroutine(coroutine);
        textObj.maxVisibleCharacters = textObj.text.Length;
    }
}
