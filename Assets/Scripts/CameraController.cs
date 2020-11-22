using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Camera;

    [SerializeField] Transform _target;
    [SerializeField] LayerMask _bounds;
    [SerializeField] Camera _mainCamera;

    void Start()
    {
        Camera = this;
    }

    void Update()
    {
        float cameraHeight = _mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * _mainCamera.aspect;

        Vector2 vertBoxSize = new Vector2(cameraWidth, 1);
        Vector2 horzBoxSize = new Vector2(1, cameraHeight);

        float maxY = Physics2D.BoxCast(transform.position, vertBoxSize, 0, Vector2.up, Mathf.Infinity, _bounds).collider.bounds.max.y - cameraHeight/2;
        float minY = Physics2D.BoxCast(transform.position, vertBoxSize, 0, Vector2.down, Mathf.Infinity, _bounds).collider.bounds.min.y + cameraHeight / 2;

        float maxX = Physics2D.BoxCast(transform.position, horzBoxSize, 0, Vector2.right, Mathf.Infinity, _bounds).collider.bounds.max.x - cameraWidth / 2;
        float minX = Physics2D.BoxCast(transform.position, horzBoxSize, 0, Vector2.left, Mathf.Infinity, _bounds).collider.bounds.min.x + cameraWidth / 2;

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, maxY));
        Debug.DrawLine(transform.position, new Vector2(transform.position.x, minY));
        Debug.DrawLine(transform.position, new Vector2(maxX, transform.position.y));
        Debug.DrawLine(transform.position, new Vector2(minX, transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Clamp(_target.position.x, minX, maxX), Mathf.Clamp(_target.position.y, minY, maxY), transform.position.z), 2);
    }

    public void CameraMoving()
    {
        Time.timeScale = 0;
    }
}
