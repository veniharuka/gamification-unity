using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public Animator animator;

    public void StartPortraitEffect()
    {
        try
        {
            animator.SetTrigger("doEffect");
            Debug.Log("PortraitEffect 애니메이션 시작");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("애니메이션 트리거 설정 중 오류 발생: " + ex.Message);
            Debug.LogError("Animator가 올바르게 할당되어 있는지 확인하세요.");
        }
    }
    public void start()
    {
        // 애니메이션 시작 시 실행할 코드
        Debug.Log("PortraitEffect 애니메이션이 시작되었습니다.");
    }

    public void end()
    {
        // 애니메이션 끝날 때 실행할 코드
        Debug.Log("PortraitEffect 애니메이션이 종료되었습니다.");
    }
}
