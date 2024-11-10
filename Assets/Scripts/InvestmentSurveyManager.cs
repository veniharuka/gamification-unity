using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InvestmentSurveyManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private Button answerButtonPrefab;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private GameObject gradePanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button startButton;

    private Button yesButton;
    private Button noButton;

    [Header("Survey Questions")]
    private string[] questions = new string[]
    {
        "최근 1년간 주식, 펀드 등 투자 경험이 있으신가요?",
        "투자 손실이 발생해도 감내할 수 있으신가요?",
        "정기적인 수입이 있으신가요?",
        "비상자금을 별도로 보유하고 계신가요?",
        "장기 투자(3년 이상)를 고려하고 계신가요?",
        "투자 관련 정보를 정기적으로 확인하시나요?",
        "분산투자의 개념을 이해하고 계신가요?",
        "투자 위험을 감수하더라도 높은 수익을 추구하시나요?",
        "정기적으로 저축이나 투자를 하고 계신가요?",
        "금융상품의 위험도를 이해하고 계신가요?"
    };

    private int currentQuestionIndex = 0;
    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        InitializeUI();
        if (!ValidateReferences())
        {
            Debug.LogError("Some references are missing. Please check the Inspector!");
            return;
        }

        SetupButtons();
        CreateAnswerButtons();
    }
    private bool ValidateReferences()
    {
        bool isValid = true;

        // Reference 체크는 하되, 초기 UI 상태에는 영향을 주지 않도록 수정
        if (answerButtonPrefab == null)
        {
            Debug.LogError("Answer Button Prefab is not assigned!");
            isValid = false;
        }

        if (answerPanel == null)
        {
            Debug.LogError("Answer Panel is not assigned!");
            isValid = false;
        }

        if (surveyPanel == null)
        {
            Debug.LogError("Survey Panel is not assigned!");
            isValid = false;
        }

        if (startPanel == null)
        {
            Debug.LogError("Start Panel is not assigned!");
            isValid = false;
        }

        if (gradePanel == null)
        {
            Debug.LogError("Grade Panel is not assigned!");
            isValid = false;
        }

        if (questionText == null)
        {
            Debug.LogError("Question Text is not assigned!");
            isValid = false;
        }

        if (gradeText == null)
        {
            Debug.LogError("Grade Text is not assigned!");
            isValid = false;
        }

        if (startButton == null)
        {
            Debug.LogError("Start Button is not assigned!");
            isValid = false;
        }

        if (confirmButton == null)
        {
            Debug.LogError("Confirm Button is not assigned!");
            isValid = false;
        }

        return isValid;
    }

    private void InitializeUI()
    {
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
            selectedAnswers[i] = -1;

        surveyPanel.SetActive(false);
        gradePanel.SetActive(false);
        startPanel.SetActive(true);
    }

    private void CreateAnswerButtons()
    {
        if (answerPanel == null || answerButtonPrefab == null) return;

        // Clear existing buttons
        foreach (Transform child in answerPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create "Yes" button
        yesButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        TextMeshProUGUI yesText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        if (yesText != null) yesText.text = "예";
        yesButton.onClick.AddListener(() => SelectAnswer(0));

        // Create "No" button
        noButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        TextMeshProUGUI noText = noButton.GetComponentInChildren<TextMeshProUGUI>();
        if (noText != null) noText.text = "아니오";
        noButton.onClick.AddListener(() => SelectAnswer(1));
    }

    private void SetupButtons()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnStartButtonClick);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        confirmButton.gameObject.SetActive(false);
    }

    private void OnStartButtonClick()
    {
        surveyPanel.SetActive(true);
        startPanel.SetActive(false);
        InitializeSurvey();
    }

    private void OnConfirmButtonClick()
    {
        ResetSurveyState();
        startPanel.SetActive(true);
        surveyPanel.SetActive(false);
        gradePanel.SetActive(false);
    }

    private void ResetSurveyState()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;
        }

        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;

        questionText.gameObject.SetActive(true);
        answerPanel.SetActive(true);
    }

    public void SelectAnswer(int answerIndex)
    {
        if (currentQuestionIndex >= questions.Length) return;

        if (selectedAnswers[currentQuestionIndex] == 0)
            yesCount--;

        selectedAnswers[currentQuestionIndex] = answerIndex;

        if (answerIndex == 0)
            yesCount++;

        HighlightSelectedButton(answerIndex == 0 ? yesButton : noButton);
        StartCoroutine(MoveToNextQuestionAfterDelay());
    }

    private IEnumerator MoveToNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowCurrentQuestion();
            ResetButtonStates();
        }
        else
        {
            CompleteSurvey();
        }
    }

    private void ResetButtonStates()
    {
        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;
    }

    private (string grade, string description) CalculateGrade()
    {
        if (yesCount <= 1) return ("6등급(매우낮은위험)", "원금 손실의 위험이 매우 낮은 상품을 선호하는 투자자");
        if (yesCount <= 3) return ("5등급(낮은위험)", "원금 손실의 위험이 낮은 상품을 선호하는 투자자");
        if (yesCount <= 5) return ("4등급(중립형)", "원금 손실과 이익 가능성이 균형을 이루는 상품을 선호하는 투자자");
        if (yesCount <= 7) return ("3등급(적극투자형)", "높은 수익을 위해 높은 위험을 감수할 수 있는 투자자");
        if (yesCount <= 9) return ("2등급(공격투자형)", "매우 높은 수익을 위해 높은 위험을 감수할 수 있는 투자자");
        return ("1등급(위험선호형)", "투자 위험을 적극적으로 수용하고 높은 수익을 추구하는 투자자");
    }

    private void CompleteSurvey()
    {
        questionText.gameObject.SetActive(false);
        answerPanel.SetActive(false);

        gradePanel.SetActive(true);
        confirmButton.gameObject.SetActive(true);

        var (grade, description) = CalculateGrade();
        gradeText.text = $"투자성향 분석 결과\n\n{grade}\n\n{description}";
    }

    private void InitializeSurvey()
    {
        currentQuestionIndex = 0;
        yesCount = 0;
        ResetButtonStates();
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        if (questionText != null && currentQuestionIndex < questions.Length)
        {
            questionText.text = $"Q{currentQuestionIndex + 1}. {questions[currentQuestionIndex]}";
        }
    }

    private void HighlightSelectedButton(Button selectedButton)
    {
        if (yesButton != null) yesButton.interactable = true;
        if (noButton != null) noButton.interactable = true;
        selectedButton.interactable = false;
    }
}