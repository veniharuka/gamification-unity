using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// 대화창 UI 구축하기 [유니티 기초 강좌 B22] - 호진
// PlayerAction 과 연결되어 있음. 
public class GameManager :  MonoBehaviour
{
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI yesText;  
    public TextMeshProUGUI noText;
    public Image arrow;

    private bool isYesSelected = true; // 현재 선택이 "예"인지 "아니오"인지 확인하는 변수


    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator protraitIAnim;
    public Image protraitImg;
    public Sprite prevPortrait;
    public TypeEffect talk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    void Start()
    {
        // 패널과 버튼을 비활성화합니다.
        choicePanel.SetActive(false);

        // 버튼 클릭 시 이벤트 연결
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    void Update()
    {
        if (choicePanel.activeSelf)
        {
            HandleArrowMovement();
        }
    }

    public void Action(GameObject scanObj) 
    {
        // Get Current Object
        scanObject = scanObj;
       ObjData objData = scanObject.GetComponent<ObjData>();
       Talk(objData.id, objData.isNpc);

        // visible talk for action
       talkPanel.SetBool("isShow", isAction);
    }
        


    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";
        // Set Talk Data
        if (talk.isAnim){ // 애니메이션 실행 중이면 
            talk.SetMsg(""); // 다음 메시지는 실행하지 않아도 됨. 
            return;
        }
        else {
             questTalkIndex = questManager.GetQuestTalkIndex(id);
             talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        
        // End Talk
        if (talkData == null) // 대화가 더 이상 없으면 종료
        {
            isAction = false;
            talkIndex = 0; // 이야기가 종료되면 초기화. 
            Debug.Log(questManager.CheckQuest(id));
            return;
        }
        // Continue Talk
        if (isNpc)
        {
            // TypeEffecter 연결
            talk.SetMsg(talkData);
            // Show Portrait
            protraitImg.color = new Color(1, 1, 1, 1); // npc라면 투명도 1(드러냄)
            // Animation Portrait  B25
            protraitIAnim.SetTrigger("doEffect");
                prevPortrait = protraitImg.sprite;
        }
        else 
        {
            // Hide Talk
            talk.SetMsg(talkData);
            protraitImg.color = new Color(1, 1, 1, 0); // npc가 아니라면 투명도 0(숨김) 
        }
        // Next Talk
        isAction = true;
        talkIndex++; // 문장 뽑아내기. 

        // 대화의 마지막 문장인지 확인
        int totalLines = talkManager.GetTalkDataLength(id + questTalkIndex);
        if (talkIndex == totalLines) // 마지막 문장일 경우
        {
            ShowChoice();
        }


    }
    void ShowChoice()
    {
        // 선택지를 활성화합니다.
        choicePanel.SetActive(true);
    }

    void OnYesButtonClicked()
    {
        Debug.Log("플레이어가 '예'를 선택했습니다.");
        choicePanel.SetActive(false);
        // 예를 선택했을 때 실행할 로직 추가
    }

    void OnNoButtonClicked()
    {
        Debug.Log("플레이어가 '아니오'를 선택했습니다.");
        choicePanel.SetActive(false);
        // 아니오를 선택했을 때 실행할 로직 추가
    }


    // 화살표 위치를 변경하는 함수
    void HandleArrowMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isYesSelected = !isYesSelected; // 선택지를 전환
            UpdateArrowPosition();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Enter 키를 누르면 선택이 확정됨
        {
            ConfirmChoice();
        }
    }

    // 화살표 위치를 업데이트하는 함수
    void UpdateArrowPosition()
    {
        if (isYesSelected)
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x, yesText.transform.position.y, 0);
        }
        else
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x, noText.transform.position.y, 0);
        }
    }

    // 선택 확정 처리
    void ConfirmChoice()
    {
        choicePanel.SetActive(false);
        if (isYesSelected)
        {
            Debug.Log("플레이어가 '예'를 선택했습니다.");
            // 예 선택 시 실행할 코드 추가
        }
        else
        {
            Debug.Log("플레이어가 '아니오'를 선택했습니다.");
            // 아니오 선택 시 실행할 코드 추가
        }
    }




}
