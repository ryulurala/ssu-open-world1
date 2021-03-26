﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.MouseEvent> MouseAction = null;
    public Action<Define.TouchEvent> TouchAction = null;
    Action _inputAction = null;
    float _pressedTime = 0f;
    Vector2 _startPos;

    public void OnAwake()
    {
        if (Util.IsMobile)
        {
            _inputAction -= OnTouchEvent;
            _inputAction += OnTouchEvent;
            MouseAction = null;
        }
        else
        {
            _inputAction -= OnMouseEvent;
            _inputAction += OnMouseEvent;
            TouchAction = null;
        }
    }

    public void OnUpdate()
    {
        // UI Click 상태
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_inputAction != null)
            _inputAction.Invoke();
    }

    public void Clear()
    {
        MouseAction = null;
        TouchAction = null;
    }

    #region Mobile
    void OnTouchEvent()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TouchAction.Invoke(Define.TouchEvent.TabWithOneStart);
                _pressedTime = Time.time;   // 시간 측정
                _startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                TouchAction.Invoke(Define.TouchEvent.PressWithOne);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - _pressedTime < Define.TouchPressedTime && (touch.position - _startPos).sqrMagnitude < Define.TouchMaxDeltaPos)
                    TouchAction.Invoke(Define.TouchEvent.TabWithOne);
            }
        }
    }
    #endregion

    #region PC
    void OnMouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseAction.Invoke(Define.MouseEvent.LeftStart);
            _pressedTime = Time.time;   // 시간 측정
        }
        else if (Input.GetMouseButton(0))
        {
            MouseAction.Invoke(Define.MouseEvent.LeftPress);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - _pressedTime < Define.MousePressedTime)
                MouseAction.Invoke(Define.MouseEvent.LeftClick);
        }

        if (Input.GetMouseButtonUp(1))
        {
            // RMB
            MouseAction.Invoke(Define.MouseEvent.RightClick);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            // Scroll Wheel
            MouseAction.Invoke(Define.MouseEvent.ScrollWheel);
        }
    }
    #endregion
}
