using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private Vector2 offset = new Vector2(300, 20);

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRectTransform)
    {
        if (!show)
        {
            rectTransform.position = new Vector3(9999, 9999);
            return;
        }
        UpdatePosition(targetRectTransform);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCentreX = Screen.width / 2;
        Vector2 targetPos = targetRect.position;

        float screenTop = Screen.height;
        float screenBottom = 0f;

        float verticalHalf = rectTransform.sizeDelta.y / 2f;

        float top = rectTransform.position.x + verticalHalf;
        float bottom = rectTransform.position.y - verticalHalf;

        if (top > screenTop)
            targetPos.y = screenTop - verticalHalf - offset.y;
        else if (bottom < screenBottom)
            targetPos.y = screenBottom + verticalHalf + offset.y;

        targetPos.x = targetPos.x > screenCentreX ? targetPos.x - offset.x : targetPos.x + offset.x;

        rectTransform.position = targetPos;
    }
    protected string GetColouredText(string color, string text)
    {
        return ($"<color={color}>{text}</color>");
    }

}
