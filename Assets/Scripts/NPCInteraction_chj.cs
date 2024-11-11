using UnityEngine;

public class NPCInteraction_chj : MonoBehaviour
{
    public GameObject dialogueUI;  // 대화 UI 패널

    void Start()
    {
        dialogueUI.SetActive(false);  // 처음에는 대화창 숨김
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueUI.SetActive(true);  // 플레이어가 범위 안에 들어오면 대화창 표시
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueUI.SetActive(false);  // 범위를 벗어나면 대화창 숨김
        }
    }
}
