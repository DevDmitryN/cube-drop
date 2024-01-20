using System;
using UnityEngine;

public class TrajectionLine : MonoBehaviour
{
   private float _lengthCoef = 3;
    
    private LineRenderer _lineRenderer;
    
    private Vector3 _fromPosition;
    
    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate(Vector3 fromPosition)
    {
        _fromPosition = fromPosition;
        
        gameObject.SetActive(true);
    }

    public void UpdateDestination(Vector3 cubePosition, float distance)
    {
        _lineRenderer.SetPosition(0, _fromPosition);
        
        var endPointPosition = (_fromPosition - cubePosition) * (distance * _lengthCoef) + _fromPosition;
        var scale = Vector3.Distance(_fromPosition, endPointPosition) / 5;

        _lineRenderer.textureScale = new Vector2(scale, 0);
        _lineRenderer.SetPosition(1, endPointPosition);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

