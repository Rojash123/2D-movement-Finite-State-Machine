using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] backGroundLayers;
    private Camera camera;
    private float lastCamerPosX;
    private float cameraHalfWidth;


    private void Start()
    {
        camera=Camera.main;
        cameraHalfWidth = camera.orthographicSize * camera.aspect;
        CalculateWidth();
    }
    private void FixedUpdate()
    {
        float currentCameraPosx=camera.transform.position.x;
        float distanceToMove = currentCameraPosx - lastCamerPosX;
        
        float cameraRightEdge = camera.transform.position.x + cameraHalfWidth;
        float cameraLeftEdge = camera.transform.position.x - cameraHalfWidth;

        lastCamerPosX = currentCameraPosx;
        foreach (var layer in backGroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackGround(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateWidth()
    {
        foreach (var layer in backGroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
