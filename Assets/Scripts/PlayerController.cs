using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb2d;
    [Space(10)]
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _ground;
    [Space(10)]

    bool isGrounded = false;

    public float jumpHeight = 2.5f;
    public float speed = 1;

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(_groundCheck.position, new Vector2(7,1), 0, _ground);

        if (Input.GetButton("Horizontal") && Time.timeScale > 0)
        {
            _rb2d.velocity = new Vector2((16*Input.GetAxis("Horizontal")) * speed, _rb2d.velocity.y);
        } else
        {
            _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && Time.timeScale > 0)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, 16 * jumpHeight);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            CameraController.Camera.CameraMoving();
        }
    }
}
