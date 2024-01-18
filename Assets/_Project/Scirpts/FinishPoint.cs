using UnityEngine;

public class FinishPoint : MonoBehaviour
{
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayCubeController>(out var cube))
        {
            cube.FinishGame();
            Debug.Log("Finish");
        }
    }
}
