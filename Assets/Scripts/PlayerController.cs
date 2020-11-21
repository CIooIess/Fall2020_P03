using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Collider2D _right;
    [SerializeField] Collider2D _left;
    [SerializeField] Collider2D _top;
    [SerializeField] Collider2D _bottom;
    [Space(10)]
    [SerializeField] Collider2D _wall;
    [SerializeField] ContactFilter2D _stage;
    [Space(10)]

    public float jumpHeight = 2.5f;
    bool isJumping = false;
    bool isFalling = false;

    void FixedUpdate()
    {
        Collider2D[] rightCheck = new Collider2D[1];
        Collider2D[] leftCheck = new Collider2D[1];
        Collider2D[] topCheck = new Collider2D[1];
        Collider2D[] bottomCheck = new Collider2D[1];

        Physics2D.OverlapCollider(_right, _stage, rightCheck);
        Physics2D.OverlapCollider(_left, _stage, leftCheck);
        Physics2D.OverlapCollider(_top, _stage, topCheck);
        Physics2D.OverlapCollider(_bottom, _stage, bottomCheck);

        if (Input.GetKey(KeyCode.A) && leftCheck[0] != _wall)
        {
            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }
        if (Input.GetKey(KeyCode.D) && rightCheck[0] != _wall)
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }

        if (bottomCheck[0] != _wall && !isJumping && !isFalling)
        {
            StartCoroutine("Falling");
        }
        if (bottomCheck[0] == _wall)
        {
            StopCoroutine("Falling");
            isFalling = false;
        }

        if (Input.GetKey(KeyCode.Space) && bottomCheck[0] == _wall && topCheck[0] != _wall)
        {
            StartCoroutine("Jumping");
        }
        if (topCheck[0] == _wall)
        {
            StopCoroutine("Jumping");
            isJumping = false;
        }
    }

    IEnumerator Jumping()
    {
        isJumping = true;
        for (float i = 0f; i < (jumpHeight*16f); i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            yield return new WaitForFixedUpdate();
        }
        isJumping = false;
        yield return null;
    }

    IEnumerator Falling()
    {
        isFalling = true;
        while (isFalling == true)
        {
            yield return new WaitForFixedUpdate();
            if (isFalling == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
        }
        yield return null;
    }
}
