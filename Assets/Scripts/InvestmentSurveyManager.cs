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

    private List<Button> currentAnswerButtons = new List<Button>();

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
    private string[] answers;

    private string[][] answerOptions = new string[][]
    {
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" },
        new string[] { "예", "아니오" }
    };

    private int[] selectedAnswers;
    private int yesCount = 0;

    void Start()
    {
        // 배열 초기화를 가장 먼저 수행
        selectedAnswers = new int[questions.Length];
        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = -1;  // -1은 아직 답변하지 않음을 의미
        }
        // GradePanel 초기에 숨김
        if (gradePanel != null)
            gradePanel.SetActive(false);
        // 확인 버튼 이벤트 리스너 추가
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();  // 기존 리스너 제거
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
        // 리스트 초기화
        currentAnswerButtons = new List<Button>();

        // 설문 초기화
        InitializeSurvey();
    }
    private void OnConfirmButtonClick()
    {
        // 설문 완료 시 패널 닫기
        surveyPanel.SetActive(false);

        // 필요한 경우 초기 상태로 리셋
        currentQuestionIndex = 0;
        yesCount = 0;

        // 버튼들 제거
        foreach (var button in currentAnswerButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
        currentAnswerButtons.Clear();
    }
    public void SelectAnswer(int answerIndex)
    {
        // 초기화 검사
        if (selectedAnswers == null)
        {
            Debug.LogError("Selected answers array is not properly initialized");
            selectedAnswers = new int[questions.Length];
            for (int i = 0; i < selectedAnswers.Length; i++)
            {
                selectedAnswers[i] = -1;
            }
        }

        if (currentQuestionIndex >= questions.Length)
        {
            Debug.LogError($"Question index {currentQuestionIndex} is out of range");
            return;
        }

        // 이전 답변이 "예"였다면 카운트 감소
        if (selectedAnswers[currentQuestionIndex] == 0)
        {
            yesCount--;
        }

        selectedAnswers[currentQuestionIndex] = answerIndex;

        // 새로운 답변이 "예"라면 카운트 증가
        if (answerIndex == 0)
        {
            yesCount++;
        }

        UpdateInvestmentGrade();

        if (currentAnswerButtons != null && answerIndex < currentAnswerButtons.Count)
        {
            HighlightSelectedButton(currentAnswerButtons[answerIndex]);
        }

        // 답변 선택 후 잠시 대기 후 다음 질문으로 이동
        StartCoroutine(MoveToNextQuestionAfterDelay());
    }
    private IEnumerator MoveToNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (currentQuestionIndex < questions.Length - 1)
        {
            currentQuestionIndex++;
            ShowCurrentQuestion();
        }
        else
        {
            CompleteSurvey();
        }
    }
    private void UpdateInvestmentGrade()
    {
        // 모든 질문에 답하기 전까지는 등급을 보여주지 않음
        if (gradeText == null) return;

        bool allQuestionsAnswered = true;
        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            if (selectedAnswers[i] == -1)
            {
                allQuestionsAnswered = false;
                break;
            }
        }

        if (!allQuestionsAnswered)
        {
            gradeText.text = "";  // 텍스트를 비움
            return;
        }

        string grade;
        string description = "";
        if (yesCount <= 1)
        {
            grade = "6등급(매우낮은위험)";
            description = "원금 손실의 위험이 매우 낮은 상품을 선호하는 투자자";
        }
        else if (yesCount <= 3)
        {
            grade = "5등급(낮은위험)";
            description = "원금 손실의 위험이 낮은 상품을 선호하는 투자자";
        }
        else if (yesCount <= 5)
        {
            grade = "4등급(중립형)";
            description = "원금 손실과 이익 가능성이 균형을 이루는 상품을 선호하는 투자자";
        }
        else if (yesCount <= 7)
        {
            grade = "3등급(적극투자형)";
            description = "높은 수익을 위해 높은 위험을 감수할 수 있는 투자자";
        }
        else if (yesCount <= 9)
        {
            grade = "2등급(공격투자형)";
            description = "매우 높은 수익을 위해 높은 위험을 감수할 수 있는 투자자";
        }
        else
        {
            grade = "1등급(위험선호형)";
            description = "투자 위험을 적극적으로 수용하고 높은 수익을 추구하는 투자자";
        }

        gradeText.text = $"투자성향 분석 결과\n\n{grade}\n\n{description}";
    }

    private void CompleteSurvey()
    {
        // 최종 결과 업데이트
        string grade = "";
        string description = "";

        if (yesCount <= 1)
        {
            grade = "6등급(매우낮은위험)";
            description = "원금 손실의 위험이 매우 낮은 상품을 선호하는 투자자";
        }
        else if (yesCount <= 3)
        {
            grade = "5등급(낮은위험)";
            description = "원금 손실의 위험이 낮은 상품을 선호하는 투자자";
        }
        else if (yesCount <= 5)
        {
            grade = "4등급(중립형)";
            description = "원금 손실과 이익 가능성이 균형을 이루는 상품을 선호하는 투자자";
        }
        else if (yesCount <= 7)
        {
            grade = "3등급(적극투자형)";
            description = "높은 수익을 위해 높은 위험을 감수할 수 있는 투자자";
        }
        else if (yesCount <= 9)
        {
            grade = "2등급(공격투자형)";
            description = "매우 높은 수익을 위해 높은 위험을 감수할 수 있는 투자자";
        }
        else
        {
            grade = "1등급(위험선호형)";
            description = "투자 위험을 적극적으로 수용하고 높은 수익을 추구하는 투자자";
        }

        // 결과 표시
        if (questionText != null)
        {
            questionText.text = $"\n\n투자성향 분석 결과\n\n{grade}\n\n{description}";
        }

        // 답변 패널 숨기기
        if (answerPanel != null)
        {
            answerPanel.SetActive(false);
        }

        Debug.Log($"설문이 완료되었습니다. 최종 등급: {grade}");
    }
    private IEnumerator CloseSurveyAfterDelay()
    {
        yield return new WaitForSeconds(3f);  // 3초 후에 닫기
        surveyPanel.SetActive(false);
    }

    private void InitializeSurvey()
    {
        if (selectedAnswers == null || selectedAnswers.Length != questions.Length)
        {
            selectedAnswers = new int[questions.Length];
            for (int i = 0; i < selectedAnswers.Length; i++)
            {
                selectedAnswers[i] = -1;
            }
        }

        currentQuestionIndex = 0;
        yesCount = 0;
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        if (questionText != null)
        {
            questionText.text = $"Q{currentQuestionIndex+1}. {questions[currentQuestionIndex]}";
        }

        // 기존 버튼들 제거
        if (currentAnswerButtons != null)
        {
            foreach (var button in currentAnswerButtons)
            {
                if (button != null)
                {
                    Destroy(button.gameObject);
                }
            }
            currentAnswerButtons.Clear();
        }

        // null 체크 추가
        if (answerPanel == null || answerButtonPrefab == null)
        {
            Debug.LogError("Answer panel or button prefab is not assigned");
            return;
        }

        string[] currentAnswers = answerOptions[currentQuestionIndex];
        for (int i = 0; i < 2; i++)
        {
            // 버튼 생성
            Button newButton = Instantiate(answerButtonPrefab, answerPanel.transform);
            newButton.name = $"AnswerButton_{i}";

            // 모든 Image 컴포넌트 찾기
            Image[] images = newButton.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                // 버튼의 직계 Image만 유지하고 나머지는 비활성화
                if (image.gameObject == newButton.gameObject)
                {
                    image.color = Color.white;
                }
                else
                {
                    image.enabled = false;
                }
            }

            // Text 설정
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = currentAnswers[i];  // "예" 또는 "아니오"
            }

            int answerIndex = i;
            newButton.onClick.AddListener(() => SelectAnswer(answerIndex));
            currentAnswerButtons.Add(newButton);
        }
    }
    private void HighlightSelectedButton(Button selectedButton)
    {
        if (currentAnswerButtons == null) return;

        foreach (var button in currentAnswerButtons)
        {
            if (button != null)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = Color.black;
                }
            }
        }

        TextMeshProUGUI selectedText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
        if (selectedText != null)
        {
            selectedText.color = Color.blue;  // 선택된 텍스트 색상 변경
        }
    }


}