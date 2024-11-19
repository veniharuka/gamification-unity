using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InvestmentSurveyManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject surveyPanel;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject gradePanel;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button answerButtonPrefab;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button urlButton;

    private Button yesButton;
    private Button noButton;
    private bool buttonsInitialized = false;

    [Header("Survey Questions")]
    private string[] questions = new string[]
    {
        "�ֱ� 1�Ⱓ �ֽ�, �ݵ� �� ���� ������ �����Ű���?",
        "���� �ս��� �߻��ص� ������ �� �����Ű���?",
        "�������� ������ �����Ű���?",
        "����ڱ��� ������ �����ϰ� ��Ű���?",
        "��� ����(3�� �̻�)�� ����ϰ� ��Ű���?",
        "���� ���� ������ ���������� Ȯ���Ͻó���?",
        "�л������� ������ �����ϰ� ��Ű���?",
        "���� ������ �����ϴ��� ���� ������ �߱��Ͻó���?",
        "���������� �����̳� ���ڸ� �ϰ� ��Ű���?",
        "������ǰ�� ���赵�� �����ϰ� ��Ű���?"
    };

    private int currentQuestionIndex = 0;
    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        if (!ValidateReferences())
        {
            Debug.LogError("Some references are missing. Please check the Inspector!");
            return;
        }
        InitializeUI();
        SetupButtons();
        // ���� �� �ٷ� ���� ����
        BeginSurvey();
    }
    private void InitializeUI()
    {
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
            selectedAnswers[i] = -1;

        // �ʱ� UI ���� ����
        surveyPanel.SetActive(true);
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);

        if (!buttonsInitialized)
        {
            CreateAnswerButtons();
            buttonsInitialized = true;
        }
    }
    private void BeginSurvey()
    {
        surveyPanel.SetActive(true);
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);
        InitializeSurvey();
    }

    private void CreateAnswerButtons()
    {
        if (answerPanel == null || answerButtonPrefab == null) return;

        // ���� ��ư�� ����
        foreach (Transform child in answerPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // �� ��ư ����
        yesButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        // ������Ʈ�� ��������� Ȱ��ȭ
        Image yesImage = yesButton.GetComponent<Image>();
        if (yesImage != null)
        {
            yesImage.enabled = true;
            yesImage.raycastTarget = true;
        }

        Button yesButtonComp = yesButton.GetComponent<Button>();
        if (yesButtonComp != null)
        {
            yesButtonComp.enabled = true;
            yesButtonComp.interactable = true;
        }

        RectTransform yesRect = yesButton.GetComponent<RectTransform>();
        yesRect.sizeDelta = new Vector2(230f, 50f);

        TextMeshProUGUI yesText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        if (yesText != null)
        {
            yesText.enabled = true;
            yesText.text = "��";
            yesText.alignment = TextAlignmentOptions.Center;
        }
        yesButton.onClick.AddListener(() => SelectAnswer(0));

        // �ƴϿ� ��ư�� ������ ������� ó��
        noButton = Instantiate(answerButtonPrefab, answerPanel.transform);
        Image noImage = noButton.GetComponent<Image>();
        if (noImage != null)
        {
            noImage.enabled = true;
            noImage.raycastTarget = true;
        }

        Button noButtonComp = noButton.GetComponent<Button>();
        if (noButtonComp != null)
        {
            noButtonComp.enabled = true;
            noButtonComp.interactable = true;
        }

        RectTransform noRect = noButton.GetComponent<RectTransform>();
        noRect.sizeDelta = new Vector2(230f, 50f);

        TextMeshProUGUI noText = noButton.GetComponentInChildren<TextMeshProUGUI>();
        if (noText != null)
        {
            noText.enabled = true;
            noText.text = "�ƴϿ�";
            noText.alignment = TextAlignmentOptions.Center;
        }
        noButton.onClick.AddListener(() => SelectAnswer(1));
    }
    private bool ValidateReferences()
    {
        if (surveyPanel == null || contentPanel == null || gradePanel == null ||
            questionText == null || answerPanel == null ||
            answerButtonPrefab == null || gradeText == null || confirmButton == null)
        {
            Debug.LogError("Required references are missing!");
            return false;
        }
        return true;
    }

    private void SetupButtons()
    {
        // Confirm ��ư ����
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        confirmButton.gameObject.SetActive(false);

        // URL ��ư ����
        urlButton.onClick.RemoveAllListeners();
        urlButton.onClick.AddListener(OnUrlButtonClick);
        urlButton.gameObject.SetActive(false);
    }
    private void OnUrlButtonClick()
    {
        Application.OpenURL("https://obank.kbstar.com/quics?page=C020710");
    }
    private void OnConfirmButtonClick()
    {
        // ���� �����
        contentPanel.SetActive(true);
        gradePanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        urlButton.gameObject.SetActive(false);    // URL ��ư ��Ȱ��ȭ
        ResetSurveyState();
        BeginSurvey();
    }

    private void ResetSurveyState()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;
        }

        if (questionText != null)
            questionText.gameObject.SetActive(true);

        if (answerPanel != null)
            answerPanel.SetActive(true);

        ResetButtonStates();
        buttonsInitialized = false;
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
        if (yesCount <= 1) return ("6���(�ſ쳷������)", "���� �ս��� ������ �ſ� ���� ��ǰ�� ��ȣ�ϴ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
        if (yesCount <= 3) return ("5���(��������)", "���� �ս��� ������ ���� ��ǰ�� ��ȣ�ϴ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
        if (yesCount <= 5) return ("4���(�߸���)", "���� �սǰ� ���� ���ɼ��� ������ �̷�� ��ǰ�� ��ȣ�ϴ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
        if (yesCount <= 7) return ("3���(����������)", "���� ������ ���� ���� ������ ������ �� �ִ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
        if (yesCount <= 9) return ("2���(����������)", "�ſ� ���� ������ ���� ���� ������ ������ �� �ִ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
        return ("1���(���輱ȣ��)", "���� ������ ���������� �����ϰ� ���� ������ �߱��ϴ� �������Դϴ�.\n\n���� ��ǰ�� �ñ��ϽŰ���?\n���� �ٷ� Ȯ���غ�����!");
    }

    private void CompleteSurvey()
    {
        // ContentPanel ��Ȱ��ȭ
        contentPanel.SetActive(false);

        // GradePanel Ȱ��ȭ �� ��� ǥ��
        gradePanel.SetActive(true);
        confirmButton.gameObject.SetActive(true);
        urlButton.gameObject.SetActive(true);    // URL ��ư Ȱ��ȭ

        var (grade, description) = CalculateGrade();
        gradeText.text = $"������ ���ڼ�����\n\n{grade}\n{description}";
    }
    private void InitializeSurvey()
    {
        currentQuestionIndex = 0;
        yesCount = 0;

        // UI �ʱ�ȭ
        questionText.gameObject.SetActive(true);
        answerPanel.SetActive(true);
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