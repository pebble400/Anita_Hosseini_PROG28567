using UnityEngine;

public class GunshipController : MonoBehaviour
{
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;

    public GameObject cannonballPrefab;
    public float cannonballForce = 150f;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 leftDirection = AimCannon(mousePosition, leftCannon);
        Vector3 rightDirection = AimCannon(mousePosition, rightCannon);
        //FireCannonball(leftCannon.right, leftCannon);
        if (Input.GetMouseButtonDown(0))
        {
            FireCannonball(leftDirection.normalized, leftCannon);
        }
        if (Input.GetMouseButtonDown(1))
        {
            FireCannonball(rightDirection.normalized, rightCannon);
        }
    }

    private Vector3 AimCannon(Vector3 target, Transform cannon)
    {
        Vector3 direction = target - cannon.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        cannon.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return direction;
    }

    private void FireCannonball(Vector3 direction, Transform cannon)
    {
        GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, Quaternion.identity);

        Rigidbody2D body2D = cannonball.GetComponent<Rigidbody2D>();
        body2D.AddForce(direction * cannonballForce, ForceMode2D.Impulse);

        //GameObject cannonBall = Instantiate(cannonballPrefab, transform.position, Quaternion.identity, cannon);
        //Rigidbody2D body2D = cannonBall.GetComponent<Rigidbody2D>();
        //if (Input.GetMouseButton(0))
        //{
        //body2D.AddForce(direction, ForceMode2D.Impulse);
        //}
    }
}
