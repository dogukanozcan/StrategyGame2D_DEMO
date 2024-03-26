using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MasterMonoBehaviour
{
    private Vector3 dragStartPosition;
    private Camera _camera;

    public bool onDrag = false;
    public Vector2 dragBorder = new Vector2(30, 30);

    public static CameraManager Instance;


    public float maxZoom = 5;
    public float minZoom = 20;
    public float speed = 30;
    private float targetZoom;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

       
        _camera = GetComponent<Camera>();
        targetZoom = _camera.orthographicSize;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            dragStartPosition = GetMousePosition();
        }

        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var diff = GetMousePosition() - transform.position;
            var newPosition = dragStartPosition - diff;
            if(newPosition.x > dragBorder.x || newPosition.y > dragBorder.y) { return; }
            if(newPosition.x < -dragBorder.x || newPosition.y < -dragBorder.y) { return; }
            var magnitude = Mathf.Abs((transform.position - newPosition).magnitude);
            if (magnitude > .1f)
                onDrag = true;

            transform.position = newPosition;

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Delay(() => onDrag = false, Time.deltaTime);
            
        }


        //zoom in-out
        targetZoom -= Input.mouseScrollDelta.y;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(_camera.orthographicSize, targetZoom, speed * Time.deltaTime);
        _camera.orthographicSize = newSize;
    }

    private Vector3 GetMousePosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
