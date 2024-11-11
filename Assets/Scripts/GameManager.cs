using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// 대화창 UI 구축하기 [유니티 기초 강좌 B22] - 호진
// PlayerAction 과 연결되어 있음. 
public class GameManager :  MonoBehaviour
{
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


            // Animation Portrait 
            // B25 | UI 이미지가 변할 때만 움직이도록 하려면 활성화. 
            //if (prevPortrait != protraitImg.sprite)
            //{
                protraitIAnim.SetTrigger("doEffect");
                prevPortrait = protraitImg.sprite;
            //}
        }
        else {
            // Hide Talk
            talk.SetMsg(talkData);
            protraitImg.color = new Color(1, 1, 1, 0); // npc가 아니라면 투명도 0(숨김) 


        }
        // Next Talk
        isAction = true;
        talkIndex++; // 문장 뽑아내기. 
    }
}
