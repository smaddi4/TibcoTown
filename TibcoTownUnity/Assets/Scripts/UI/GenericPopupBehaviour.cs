using System;
using UnityEngine;
using UnityEngine.UI;

public class GenericPopupBehaviour : UIBehaviour
{
    [SerializeField] 
    private GameObject popup;

    [SerializeField]
    private Text
        header,
        description,
        submitBtnText;
    
    [SerializeField] 
    private GameObject backBtn, correct, incorrect;

    [SerializeField] 
    private Color defaultColor;
    
    private Action 
        onSubmit, 
        onBack;

    public static GenericPopupBehaviour Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnButtonClick(string button)
    {
        switch (button)
        {
            case "Button-Submit":
                onSubmit?.Invoke();
                popup.SetActive(false);
                break;
        
            case "Button-Back":
                onBack?.Invoke();
                popup.SetActive(false);
                break;
        }
    }

    public void SetValues(string header, string message, string submitBtnText, Action onSubmit = default, Action onBack = default, bool   isBackBtnRequires = false)
    {
        correct.SetActive(false);
        incorrect.SetActive(false);
        backBtn.SetActive(isBackBtnRequires);
        this.header.text        = header;
        this.description.text   = message;
        this.submitBtnText.text = submitBtnText;
        this.onSubmit           = onSubmit;
        this.onBack             = onBack;
        this.description.alignment = TextAnchor.MiddleCenter;
        popup.SetActive(true);
    }
    
    public void SetValues(bool isCorrect, Action onSubmit = default, Action onBack = default)
    {
        backBtn.SetActive(false);
        header.text        = isCorrect ? "Correct!" : "Incorrect!";
        description.text   = $"You have selected {(isCorrect ? "correct" : "incorrect")} answer.";
        submitBtnText.text = "OKAY";
        correct.SetActive(isCorrect);
        incorrect.SetActive(!isCorrect);
        this.onSubmit           = onSubmit;
        this.onBack             = onBack;
        description.alignment = TextAnchor.LowerCenter;
        popup.SetActive(true);
    }

    public override void GameStateChanged(GameState gameState)
    {
    }
}
