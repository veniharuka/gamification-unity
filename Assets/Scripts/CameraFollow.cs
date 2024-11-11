using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // 플레이어 오브젝트를 참조하는 변수

    void Update()
    {
        // 카메라 위치를 플레이어의 위치로 설정
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
