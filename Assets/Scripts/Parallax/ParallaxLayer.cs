using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform backGround;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset;

    private float imageFullWidth, imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth = backGround.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        backGround.transform.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    public void LoopBackGround(float camerLeftEdge, float camerRightEdge)
    {
        float imageRightEdge = (backGround.transform.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (backGround.transform.position.x - imageHalfWidth) + imageWidthOffset;

        if (imageRightEdge < camerLeftEdge)
            backGround.transform.position += Vector3.right * imageFullWidth;
        if (imageLeftEdge > camerRightEdge)
            backGround.transform.position += Vector3.right * -imageFullWidth;

    }
}
