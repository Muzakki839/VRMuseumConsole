using UnityEngine;

public class CanScoreTrigger : MonoBehaviour
{
    private bool scored = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!scored && other.CompareTag("PointZone"))
        {
            scored = true;
            SM_LemparBola_Manager.Instance.AddScore(1);
        }
    }
}
