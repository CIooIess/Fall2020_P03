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
        if (!CameraController.Camera.cameraPause)
        {
            if (_rb2d.simulated != true)
            {
                _rb2d.simulated = true;
            }

            isGrounded = Physics2D.OverlapBox(_groundCheck.position, new Vector2(7.9f, 1), 0, _ground);

            if (Input.GetButton("Horizontal"))
            {
                _rb2d.velocity = new Vector2((16 * Input.GetAxis("Horizontal")) * speed, _rb2d.velocity.y);
            }
            else
            {
                _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
            }

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                _rb2d.velocity = new Vector2(_rb2d.velocity.x, 16 * jumpHeight);
            }
        }
        else
        {
            _rb2d.simulated = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") && !CameraController.Camera.cameraPause) //only works if entering a "wall" trigger, and the game is not paused due to camera mechanic!
        {
            bool horz = false;
            bool vert = false;

            if (transform.position.x < other.bounds.min.x) //left side check
            {
                transform.position = new Vector2(other.bounds.min.x + 24, transform.position.y);
                horz = true; //horizontal entry
            }
            if (transform.position.x > other.bounds.max.x) //right side check
            {
                transform.position = new Vector2(other.bounds.max.x - 24, transform.position.y);
                horz = true; //horizontal entry
            }
            if (transform.position.y < other.bounds.min.y) //bottom check
            {
                transform.position = new Vector2(transform.position.x, other.bounds.min.y + 24);
                vert = true; //vertical entry
            }
            if (transform.position.y > other.bounds.max.y) //top check
            {
                transform.position = new Vector2(transform.position.x, other.bounds.max.y - 24);
                vert = true; //vertical entry
            }

            if (horz || vert) //will only start the coroutine if entering at all! (prevents random triggering)
            {
                CameraController.Camera.StartCoroutine(CameraController.Camera.CameraMoving(vert,horz));
            }
        }
    }
}
