using UnityEngine;
using UnityEngine.Rendering;

// B24 
public class QuestData
{
    public string questName;
    public int[] npcId;

    public QuestData(string name, int[] npc)
    { 
        questName = name;
        npcId = npc;
    
    }
}
