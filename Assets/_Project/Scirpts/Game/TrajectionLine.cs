using System;
using System.Linq;
using UnityEngine;

public class TrajectionLine : MonoBehaviour
{
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

    public void UpdateDestination(Vector3 cubePosition, float distance, float velocityDropCoefficient)
    {
        velocityDropCoefficient = 3;
        // _lineRenderer.SetPosition(0, _fromPosition);
       
        
        var velocity = (_fromPosition - cubePosition) * (distance * velocityDropCoefficient);
        
        var endPointPosition = (_fromPosition - cubePosition) * (distance * velocityDropCoefficient) + _fromPosition;

        var angle = Mathf.Atan2( velocity.y, velocity.x);
        
        //Debug.Log($"{velocity.magnitude} Angle = {angle * Mathf.Rad2Deg} {Physics.gravity}");
        
        var scale = Vector3.Distance(_fromPosition, endPointPosition) / 2;

        _lineRenderer.textureScale = new Vector2(0.1f, 0);
        
        _lineRenderer.SetPosition(0, _fromPosition);
        
        
        // _lineRenderer.SetPosition(1,  GetPositionAtTime(velocity, 0.5f, angle, cubePosition));
        // _lineRenderer.SetPosition(1, GetPositionAtTime(velocity.magnitude, 0.5f, angle));
        // // _lineRenderer.SetPosition(3, GetPositionAtTime(velocity, 1.5f, angle, cubePosition));
        // _lineRenderer.SetPosition(2, GetPositionAtTime(velocity.magnitude, 1, angle));
        // _lineRenderer.SetPosition(3, GetPositionAtTime(velocity.magnitude, 1.5f, angle));
        // _lineRenderer.SetPosition(4, GetPositionAtTime(velocity.magnitude, 2, angle));
        // _lineRenderer.SetPosition(5, GetPositionAtTime(velocity.magnitude, 2.5f, angle));
        // _lineRenderer.SetPosition(6, GetPositionAtTime(velocity.magnitude, 3, angle));

        var timeValues = Enumerable.Range(0, _lineRenderer.positionCount).ToList();
        
        for (var i = 1; i < timeValues.Count; i++)
        {
            _lineRenderer.SetPosition(i, GetPositionAtTime(velocity.magnitude, (float)timeValues[i] * 2 /timeValues.Count, angle));
        }
     
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private Vector3 GetPositionAtTime(float velocityMagnitude, float t, float angle)
    {
        
        var x = (velocityMagnitude * t * Mathf.Cos(angle));
        var y = (velocityMagnitude * t * Mathf.Sin(angle) - (1.3f * Mathf.Pow(t, 2)) / 2);

        return new Vector3(x,  y, _fromPosition.z) + _fromPosition;
    }
    
}

