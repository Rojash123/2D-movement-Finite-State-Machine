using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{

    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLenth;
    [SerializeField] private RectTransform childNodeConnectionPoint;

   
    public Image GetConnectionImage()=>connectionLenth.GetComponent<Image>();

    public void DirectConnection(NodeDirectionType type, float length,float offset)
    {
        rotationPoint.transform.Rotate(0, 0, GetAngle(type));

        bool shouldBeActive = type != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float zAngle = GetAngle(type);

        rotationPoint.transform.localRotation = Quaternion.Euler(0, 0, zAngle+offset);
        connectionLenth.sizeDelta =new Vector2(finalLength,connectionLenth.sizeDelta.y);
    }

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out Vector2 localPosition
            );
        return localPosition;
    }

    private float GetAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.Right: return 0;
            case NodeDirectionType.upRight: return 45;
            case NodeDirectionType.up: return 90;
            case NodeDirectionType.upLeft: return 145;
            case NodeDirectionType.Left: return 180;
            case NodeDirectionType.DownLeft: return 225;
            case NodeDirectionType.Down: return 270;
            case NodeDirectionType.DownRight: return 315;
            default: return 0;
        }
    }


}

public enum NodeDirectionType
{
    None,
    upLeft,
    up,
    upRight,
    Left,
    Right,
    Down,
    DownLeft,
    DownRight
}
