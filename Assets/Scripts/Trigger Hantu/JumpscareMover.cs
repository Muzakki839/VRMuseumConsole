using UnityEngine;

public class JumpscareMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointB;
    public float speed = 2f;
    public float destroyDelay = 1f;

    private Vector3 pointA;
    private bool isMoving = false;

    private void Awake()
    {
        // Simpan posisi awal sebagai pointA
        pointA = transform.position;
    }

    public void StartMoving()
    {
        if (pointB == null)
        {
            Debug.LogWarning("JumpscareMover: PointB belum diassign.");
            return;
        }

        transform.position = pointA;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {
            isMoving = false;
            Destroy(gameObject, destroyDelay);
        }
    }
}
