using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController Camera;
    public bool cameraPause = false;

    [SerializeField] Transform _target;
    [SerializeField] LayerMask _bounds;
    [SerializeField] Camera _mainCamera;
    [SerializeField] Image _fadeBox;
    [SerializeField] Text _fps;

    public float fadeTimer = 1f;

    void Start()
    {
        Camera = this;
    }

    void Update()
    {
        if (!cameraPause)
        {
            float cameraHeight = _mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _mainCamera.aspect;

            float maxY = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, _bounds).collider.bounds.max.y - cameraHeight / 2;
            float minY = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, _bounds).collider.bounds.min.y + cameraHeight / 2;

            float maxX = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, _bounds).collider.bounds.max.x - cameraWidth / 2;
            float minX = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, _bounds).collider.bounds.min.x + cameraWidth / 2;

            Debug.DrawLine(transform.position, new Vector2(transform.position.x, maxY));
            Debug.DrawLine(transform.position, new Vector2(transform.position.x, minY));
            Debug.DrawLine(transform.position, new Vector2(maxX, transform.position.y));
            Debug.DrawLine(transform.position, new Vector2(minX, transform.position.y));

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Clamp(_target.position.x, minX, maxX), Mathf.Clamp(_target.position.y, minY, maxY), transform.position.z), 2);
        }

        _fps.text = (1.0f / Time.smoothDeltaTime).ToString();
    }

    public IEnumerator CameraMoving(bool vert, bool horz)
    {
        float cameraHeight = _mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * _mainCamera.aspect;

        float maxY = Physics2D.Raycast(_target.position, Vector2.up, Mathf.Infinity, _bounds).collider.bounds.max.y - cameraHeight / 2;
        float minY = Physics2D.Raycast(_target.position, Vector2.down, Mathf.Infinity, _bounds).collider.bounds.min.y + cameraHeight / 2;

        float maxX = Physics2D.Raycast(_target.position, Vector2.right, Mathf.Infinity, _bounds).collider.bounds.max.x - cameraWidth / 2;
        float minX = Physics2D.Raycast(_target.position, Vector2.left, Mathf.Infinity, _bounds).collider.bounds.min.x + cameraWidth / 2;

        cameraPause = true;

        int colorSteps = Mathf.FloorToInt(fadeTimer / 0.02f);

        for (int i = 0; i <= colorSteps; i++)
        {
            _fadeBox.color = new Color(_fadeBox.color.r, _fadeBox.color.g, _fadeBox.color.b, (1f / colorSteps) * i);
            yield return new WaitForFixedUpdate();
        }

        Vector3 targetPos = new Vector3(Mathf.Clamp(_target.position.x, minX, maxX), Mathf.Clamp(_target.position.y, minY, maxY), transform.position.z);

        while (transform.position != targetPos)
        {
            if (vert)
            {
                while (transform.position.x != targetPos.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), 400 * Time.deltaTime);
                    yield return null;
                }
            }
            if (horz)
            {
                while (transform.position.y != targetPos.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPos.y, transform.position.z), 400 * Time.deltaTime);
                    yield return null;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 400 * Time.deltaTime);
            yield return null;
        }

        for (int i = colorSteps; i >= 0; i--)
        {
            _fadeBox.color = new Color(_fadeBox.color.r, _fadeBox.color.g, _fadeBox.color.b, (1f / colorSteps) * i);
            yield return new WaitForFixedUpdate();
        }

        cameraPause = false;
        Debug.Log("done");
    }
}
