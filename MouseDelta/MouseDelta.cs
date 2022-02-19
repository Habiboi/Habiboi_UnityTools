using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDelta : MonoBehaviour
{
    public static MouseDelta instance;

    private Touch touch;
    private Vector2 currentPos, lastPos, deltaPos;

    private float width, height;
    [HideInInspector] public float deltaX, deltaY;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    private void Update()
    {
        SetMouseDelta();
    }

    public void SetMouseDelta()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            touch = Input.GetTouch(0);
            deltaPos = touch.deltaPosition;
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            currentPos = Input.mousePosition;
            deltaPos = currentPos - lastPos;
            lastPos = currentPos;
        }

        deltaX = deltaPos.x / width;
        deltaY = deltaPos.y / height;
    }
}
