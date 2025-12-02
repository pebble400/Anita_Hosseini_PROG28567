using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))] 
public class FollowCamera2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Tilemap tilemap;

    
    private Vector3 offset;
    

    private float leftCameraBoundary;
    private float rightCameraBoundary;
    private float bottomCameraBoundary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - target.position;
        CalculateBoundes();
    }

    private void CalculateBoundes()
    {
        tilemap.CompressBounds();

        Camera cam = GetComponent<Camera>();

        float orthoSize = cam.orthographicSize;
        Vector3 viewportHalfSize = new(orthoSize * cam.aspect, orthoSize);

        Vector3Int Min = tilemap.cellBounds.min;
        Vector3Int Max = tilemap.cellBounds.max;

        leftCameraBoundary = Min.x + viewportHalfSize.x;
        rightCameraBoundary = Max.x - viewportHalfSize.x;
        bottomCameraBoundary = Min.y + viewportHalfSize.y;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 steppedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);

        steppedPosition.x = Mathf.Clamp(steppedPosition.x, leftCameraBoundary, rightCameraBoundary);
        steppedPosition.y = Mathf.Clamp(steppedPosition.y, bottomCameraBoundary, steppedPosition.y);

        transform.position = steppedPosition;
    }
}
