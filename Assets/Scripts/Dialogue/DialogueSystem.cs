using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum STATE
{
    DISABLED,
    WAITING,
    TYPING
}

public class DialogueSystem : MonoBehaviour
{
    STATE state;
    public DialogueData dialogueData;
    int current = 0;
    bool ended = false;

    private TypeTextAnimation typeText;
    private DialogueUI dialogueUI;

    void Awake()
    {
        Transform dialogueContainer = transform.parent;
        dialogueUI = dialogueContainer.GetChild(1).GetComponent<DialogueUI>();

        typeText = GetComponent<TypeTextAnimation>();

        typeText.TypeFinished = OnTypeFinished;
        for (int i = 0; i < dialogueData.talkScript.Count; i++)
        {
            if (dialogueData.talkScript[i].name == "Protagonista")
            {
                Dialogue temp = dialogueData.talkScript[i];
                temp.name = PlayerPrefs.GetString("PlayerName", "Protagonista");
                dialogueData.talkScript[i] = temp;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = STATE.DISABLED;
        Next();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        } 
    }

    public void Next()
    {
        if (current == 0)
        {
            dialogueUI.Enable();
        }

        typeText.speaker = dialogueData.talkScript[current].name;
        typeText.text = dialogueData.talkScript[current++].text;
        if (current == dialogueData.talkScript.Count)
            ended = true;
        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinished()
    {
        state = STATE.WAITING;
    }

    void Waiting()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (!ended)
            {
                Next();
            }
            else
            {
                for (int i = 0; i < dialogueData.talkScript.Count; i++)
                {
                    if (dialogueData.talkScript[i].name == PlayerPrefs.GetString("PlayerName"))
                    {
                        Dialogue temp = dialogueData.talkScript[i];
                        temp.name = "Protagonista";
                        dialogueData.talkScript[i] = temp;
                    }
                }
                dialogueUI.Disable();
                state = STATE.DISABLED;
                current = 0;
                ended = false;
            }
        }
    }

    void Typing()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            typeText.Skip();
            state = STATE.WAITING;
        }
    }
}
