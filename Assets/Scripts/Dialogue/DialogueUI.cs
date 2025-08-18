using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    Image background;
    TextMeshProUGUI nameText;
    TextMeshProUGUI talkText;

    public float speed = 10f;
    bool open = false;

    void Awake()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        nameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        talkText = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 1, speed * Time.deltaTime);
        }
        else
        {
            background.fillAmount = Mathf.Lerp(background.fillAmount, 0, speed * Time.deltaTime);
        }
    }

    public void Enable()
    {
        background.fillAmount = 0;
        open = true;
    }

    public void Disable()
    {
        open = false;
        nameText.text = "";
        talkText.text = "";
    }
}
