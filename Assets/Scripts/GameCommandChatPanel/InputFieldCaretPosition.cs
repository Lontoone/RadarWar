using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ref:https://answers.unity.com/questions/847294/how-to-get-caret-position-inside-the-inputfield-po.html
public class InputFieldCaretPosition : InputField
{
    /*
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (isFocused)
        {
            Vector2 mPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out mPos);
            Vector2 cPos = GetLocalCaretPosition();
            Debug.Log("C:" + cPos + "  M:" + mPos);
        }
        base.OnPointerDown(eventData);
    }
    */
    public Vector2 GetLocalCaretPosition()
    {
        if (isFocused)
        {
            TextGenerator gen = m_TextComponent.cachedTextGenerator;

            int _carcet_index = caretPosition - 1;

            if (_carcet_index < 0)
                return new Vector2(0f, 0f);

            UICharInfo charInfo = gen.characters[_carcet_index];
            float x = (charInfo.cursorPos.x + charInfo.charWidth) / m_TextComponent.pixelsPerUnit;
            float y = (charInfo.cursorPos.y) / m_TextComponent.pixelsPerUnit;
            return new Vector2(x, y);
        }
        else
            return new Vector2(0f, 0f);
    }

    public void MoveCarcetToLast() {
        TextGenerator gen = m_TextComponent.cachedTextGenerator;
        UICharInfo charInfo = gen.characters[caretPosition];

        caretPosition = gen.characterCount - 1;
    }
}
