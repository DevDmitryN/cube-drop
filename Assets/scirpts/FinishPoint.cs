using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private bool _isCubeEnter;

    public bool IsCubeEnter() 
    {
        return _isCubeEnter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayCubeController>())
        {
            _isCubeEnter = true;
            Debug.Log("Finish");
        }
    }
}
