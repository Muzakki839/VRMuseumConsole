using UnityEngine;

public class TargetMover : MonoBehaviour
{
    private Vector3 moveDir;
    private float speed;

    public void Init(Vector3 dir, float spd)
    {
        moveDir = dir;
        speed = spd;
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 20f)
            Destroy(gameObject);
    }
}
