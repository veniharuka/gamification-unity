using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// B25 대화창 텍스트 이펙터 - 호진
public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;
    public int CharPerSeconds;
    public bool isAnim;

    TextMeshProUGUI msgText;
    AudioSource audioSource;
    string targetMsg;
    int index;
    float interval;

    private void Awake()
    {
        msgText = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        if (msgText == null)
        {
            Debug.LogError("msgText가 할당되지 않았습니다. TextMeshProUGUI 컴포넌트를 확인하세요.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 할당되지 않았습니다. AudioSource 컴포넌트를 확인하세요.");
        }

        if (EndCursor == null)
        {
            Debug.LogError("EndCursor가 할당되지 않았습니다. EndCursor 오브젝트를 확인하세요.");
        }
        else
        {
            // HideFlags 초기화
            EndCursor.hideFlags = HideFlags.None;
        }
    }

    public void SetMsg(string msg)
    {
        if (isAnim) {
            msgText.text = targetMsg; // 글자 마저 채우기 
            CancelInvoke(); // Invoke 바로 캔슬 
            EffectEnd(); // Effect 종료 
        }
        else {
            targetMsg = msg;
            interval = 1.0f / CharPerSeconds; // 한 번만 interval 계산
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);

        isAnim = true; 

        // Start Animation
        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        // End Animation
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        
        // Sound (공백 또는 .은 음성 제거)
        if(targetMsg[index] != ' ' || targetMsg[index] != '.')
            audioSource.Play();

        index++; // 뒤에 와야함. 

        // Recursive
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        EndCursor.SetActive(true);
    }
}
