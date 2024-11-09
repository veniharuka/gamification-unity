using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement_chj : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D를 가져옵니다
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveInput * moveSpeed;  // Rigidbody2D의 velocity로 이동을 설정
        }
        else
        {
            rb.linearVelocity = Vector2.zero;  // 이동을 멈춥니다
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            canMove = false;  // NPC와 충돌 시 이동을 멈춤
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            canMove = true;  // NPC와의 충돌에서 벗어나면 다시 이동 가능
        }
    }
}
