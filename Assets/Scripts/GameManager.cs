using TMPro;
using UnityEngine;

// 대화창 UI 구축하기 [유니티 기초 강좌 B22] - 호진
// PlayerAction 과 연결되어 있음. 
public class GameManager :  MonoBehaviour
{

    public GameObject talkPanel;
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public bool isAction;

    public void Action(GameObject scanObj) 
    {
        if (isAction){
            isAction = false;
        }
        else {
            isAction = true;
            scanObject = scanObj;
            talkText.text = "이것의 이름은" + scanObj.name + "이라고 한다";
        }

        talkPanel.SetActive(isAction);
       
    }
}
