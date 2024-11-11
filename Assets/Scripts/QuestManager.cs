using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public int questId; // 현재 진행중인 퀘스트 
    public int questActionIndex;
    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();

    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기"
                                        , new int[] { 100, 200 })); // 퀘스트 이름,  npcId
        questList.Add(20, new QuestData("억수르 찾기(금융 OX 퀴즈)"
                                        , new int[] { 1000, 2000 })); // 퀘스트 이름,  npcId
        questList.Add(30, new QuestData("성향분석 퀘스트"
                                        , new int[] { 3000, 4000 })); // 퀘스트 이름,  npcId


    }

    // NPC Id를 받고 퀘스트번호를 반환하는 함수 
    public int GetQuestTalkIndex(int id)
    {
        //return questId; 
        return questId + questActionIndex;

    }
    public string CheckQuest(int id)
    {
        // 전달된 NPC ID가 현재 퀘스트의 대상 NPC와 같으면, 퀘스트 진행 단계를 하나 증가시킴.
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // 모든 대화가 완료되면, NextQuest()를 호출하여 다음 퀘스트로 이동. 
        if (questActionIndex == questList[questId].npcId.Length)  // 퀘스트 대화 순서가 끝에 도달했을 떄 퀘스트번호 증가. 
            NextQuest(); // 다음 퀘스트 진행. 

        return questList[questId].questName; // 현재 퀘스트 이름 확인. 

    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0; 
    }

}



