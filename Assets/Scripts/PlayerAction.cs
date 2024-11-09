using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    Rigidbody2D rigid;
    float h;
    float v;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rigid.linearVelocity = new Vector2(h, v);
    }
}
