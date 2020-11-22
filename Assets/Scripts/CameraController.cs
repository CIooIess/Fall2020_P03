using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController Camera;

    [SerializeField] Transform _target;
    [SerializeField] LayerMask _bounds;
    [SerializeField] Camera _mainCamera;
    [SerializeField] Image _fadeBox;

    float cameraHeight;
    float cameraWidth;

    void Start()
    {
        Camera = this;
    }

    void Update()
    {
        cameraHeight = _mainCamera.orthographicSize * 2;
        cameraWidth = cameraHeight * _mainCamera.aspect;

        float maxY = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, _bounds).collider.bounds.max.y - cameraHeight / 2;
        float minY = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, _bounds).collider.bounds.min.y + cameraHeight / 2;

        float maxX = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, _bounds).collider.bounds.max.x - cameraWidth / 2;
        float minX = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, _bounds).collider.bounds.min.x + cameraWidth / 2;

        if (Time.timeScale > 0)
        {
            Debug.DrawLine(transform.position, new Vector2(transform.position.x, maxY));
            Debug.DrawLine(transform.position, new Vector2(transform.position.x, minY));
            Debug.DrawLine(transform.position, new Vector2(maxX, transform.position.y));
            Debug.DrawLine(transform.position, new Vector2(minX, transform.position.y));

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Clamp(_target.position.x, minX, maxX), Mathf.Clamp(_target.position.y, minY, maxY), transform.position.z), 2);
        }
    }

    public IEnumerator CameraMoving(bool horz, bool vert)
    {
        cameraHeight = _mainCamera.orthographicSize * 2;
        cameraWidth = cameraHeight * _mainCamera.aspect;

        float maxY = Physics2D.Raycast(_target.position, Vector2.up, Mathf.Infinity, _bounds).collider.bounds.max.y - cameraHeight / 2;
        float minY = Physics2D.Raycast(_target.position, Vector2.down, Mathf.Infinity, _bounds).collider.bounds.min.y + cameraHeight / 2;

        float maxX = Physics2D.Raycast(_target.position, Vector2.right, Mathf.Infinity, _bounds).collider.bounds.max.x - cameraWidth / 2;
        float minX = Physics2D.Raycast(_target.position, Vector2.left, Mathf.Infinity, _bounds).collider.bounds.min.x + cameraWidth / 2;

        Time.timeScale = 0;
        for (int i = 0; i <= 255; i++)
        {
            _fadeBox.color = new Color(_fadeBox.color.r, _fadeBox.color.g, _fadeBox.color.b, (1f/255f)*i);
            yield return new WaitForSecondsRealtime(0.001f);
        }

        Vector3 targetPos = new Vector3(0,0,-10);
        if (horz)
        {
            targetPos = new Vector3(Mathf.Clamp(_target.position.x, minX, maxX), _target.position.y, transform.position.z);
        }
        if (vert)
        {
            targetPos = new Vector3(_target.position.x, Mathf.Clamp(_target.position.y, minY, maxY), transform.position.z);
        }

        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1);
            yield return new WaitForSecondsRealtime(0.001f);
        }
        for (int i = 255; i >= 0; i--)
        {
            _fadeBox.color = new Color(_fadeBox.color.r, _fadeBox.color.g, _fadeBox.color.b, (1f / 255f) * i);
            yield return new WaitForSecondsRealtime(0.001f);
        }
        Time.timeScale = 1;
        Debug.Log("done");
    }
}
