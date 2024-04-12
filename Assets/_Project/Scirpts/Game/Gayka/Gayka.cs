using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Gayka : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DORotate(new Vector3(0,0, 360), 3, RotateMode.FastBeyond360)
            .SetRelative()
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed)
            .SetLoops(-1, LoopType.Incremental);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
