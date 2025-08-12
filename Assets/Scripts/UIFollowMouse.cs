using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public RectTransform cursorUI;
    public Canvas canvas;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Application.isFocused)
        {
            Cursor.visible = false;
            cursorUI.gameObject.SetActive(true);

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out mousePos
            );

            cursorUI.localPosition = mousePos;
        }
        else
        {
            Cursor.visible = true;
            cursorUI.gameObject.SetActive(false);
        }
    }

}
