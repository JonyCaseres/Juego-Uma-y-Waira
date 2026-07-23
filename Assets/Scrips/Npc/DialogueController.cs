using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    [Header("Choice References")]
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string name, Sprite portrait)
    {
        nameText.text = name;
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateChoiceButton(string choiceText, UnityAction onClick)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
        
        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = choiceText;
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(onClick);
        }

        return buttonObj;
    }
}
