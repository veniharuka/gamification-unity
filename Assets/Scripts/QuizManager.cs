using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button oButton;
    public Button xButton;

    public Button linkButton1;
    public Button linkButton2;
    public Image financeImage;

    private string[] questions = {
        "주식은 회사의 소유권을 나타낸다.",
        "채권은 주식보다 위험하다.",
        "인플레이션은 화폐 가치가 하락하는 현상이다.",
        "적금은 매월 일정 금액을 저축하는 방법이다.",
        "신용카드의 이자는 항상 동일하다.",
        "예금은 원금 보장이 된다.",
        "비트코인은 중앙은행이 발행한다.",
        "주식 배당금은 회사 이익에 따라 달라진다.",
        "대출을 받을 때는 반드시 담보가 필요하다.",
        "환율이 오르면 수출이 유리해진다."
    };
    private bool[] answers = { true, false, true, true, false, true, false, true, false, true };

    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        ShowQuestion();
        linkButton1.gameObject.SetActive(false);
        linkButton2.gameObject.SetActive(false);
        financeImage.gameObject.SetActive(false);
    }

    public void OnOButtonClicked()
    {
        CheckAnswer(true);
    }

    public void OnXButtonClicked()
    {
        CheckAnswer(false);
    }

    public void OpenLink1()
    {
        Application.OpenURL("https://www.kbstar.com/");
    }

    public void OpenLink2()
    {
        Application.OpenURL("https://link2.com");
    }

    void ShowQuestion()
    {
        questionText.text = questions[currentQuestionIndex];
    }

    void CheckAnswer(bool playerAnswer)
    {
        if (playerAnswer == answers[currentQuestionIndex])
        {
            score++;
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            ShowResults();
        }
    }

    void ShowResults()
    {
        questionText.text = "퀴즈가 끝났습니다! 최종 점수: " + score + "/" + questions.Length;

        // O, X 버튼 비활성화하고, 링크 버튼과 이미지를 활성화합니다.
        oButton.gameObject.SetActive(false);
        xButton.gameObject.SetActive(false);

        linkButton1.gameObject.SetActive(true);
        linkButton2.gameObject.SetActive(true);
        financeImage.gameObject.SetActive(true);
    }
}
