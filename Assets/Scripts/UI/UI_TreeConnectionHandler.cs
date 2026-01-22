using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class UI_TreeConnectionDetails
{
    public UI_TreeConnectionHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float range;
    [Range(-25f, 25f)] public float rotation;
}

public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform rectTransform => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectionDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;
    private void Awake()
    {
        if (connectionImage != null)
            originalColor = connectionImage.color;
    }
    private void OnValidate()
    {
        if (details.Length <= 0) return;

        if (details.Length != connections.Length)
        {
            Debug.LogError("Connection and data should have equal number");
        }
        UpdateConnection();
    }
    public void UpdateConnection()
    {
        for (int i = 0; i < details.Length; i++)
        {
            connections[i].DirectConnection(details[i].direction, details[i].range, details[i].rotation);
            Vector2 targetPos = connections[i].GetConnectionPoint(rectTransform);

            Image connectionImage = connections[i].GetConnectionImage();

            if (details[i].childNode == null) continue;

            details[i].childNode?.SetPosition(targetPos);
            details[i].childNode?.SetConnectionImage(connectionImage);
            details[i].childNode.transform.SetAsLastSibling();
        }
    }
    public void UpdateALlTreeConnections()
    {
        UpdateConnection();

        foreach (var connection in details)
        {
            if (connection.childNode == null) continue;
            connection.childNode?.UpdateConnection();
        }
    }

    public void UnlockConnectionImage(bool isUnlocked)
    {
        if (connectionImage == null) return;

        connectionImage.color = isUnlocked ? Color.white : originalColor;

    }

    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rectTransform.anchoredPosition = position;
}
