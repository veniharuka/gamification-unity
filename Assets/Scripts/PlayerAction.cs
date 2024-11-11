using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public GameManager manager;
    Rigidbody2D rigid;
    float h;
    float v;
    GameObject scanObject;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move Value
         h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
         v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        // Check Button Down & Up
        bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isAction ? false :Input.GetButtonUp("Vertical");

        // Check Horizontal Move
        //if (hDown)
        //  isHorizonMove = true;
        //else if (vDown)
        //  isHorizonMove = false;
        //else if (hUp || vUp)
        //  isHorizonMove = (h != 0);

        // Animation
        //if (anim.GetInteger("hAxisRaw") != h)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("hAxisRaw", (int)h);
        //}
        //else if (anim.GetInteger("vAxisRaw") != v)
        //{
        //    anim.SetBool("isChange", true);
        //    anim.SetInteger("vAxisRaw", (int)v);
        //}
        //else
        //{
        //    anim.SetBool("isChange", false);
        //}

        // Direction
        //if (vDown && v == 1) dirVec = Vector3.up;
        //else if (vDown && v == -1) dirVec = Vector3.down;
        //else if (hDown && h == -1) dirVec = Vector3.left;
        //else if (hDown && h == 1) dirVec = Vector3.right;

        // Scan Object (for interaction)
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            //Debug.Log("This is: " + scanObject.name);
            manager.Action(scanObject);
        }
    }


    void FixedUpdate()
    {
        rigid.linearVelocity = new Vector2(h, v);
    }
}
