using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class WireAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    private const float MAX_DELTA = 0.3f;
    private const float SPEED = 0.1f;
    private const float TRANSPARENCY_RATIO = 2f;

    private List<GameObject> _segments;
    private int _amount;
    private float _distance;
    private float _deltaShift;
    private PathCreator _pathCreator;

    private void Awake()
    {
        _segments = new List<GameObject>();
        _pathCreator = GetComponent<PathCreator>();
        _amount = (int)(_pathCreator.path.length / MAX_DELTA);
        _deltaShift = _pathCreator.path.length / _amount;

        for (int i = 0; i < _amount; i++)
        {
            var segment = Instantiate(_prefab);
            _segments.Add(segment);
            segment.transform.parent = transform;
        }
    }

    private void Start()
        => StartCoroutine(WireAnimationRoutine());

    private IEnumerator WireAnimationRoutine()
    {
        while (true)
        {
            var delta = 0f;
            _distance += SPEED * Time.fixedDeltaTime;
            foreach (var segment in _segments)
            {
                var distance = _distance + delta;
                segment.transform.position = _pathCreator.path.GetPointAtDistance(distance);

                var direction = _pathCreator.path.GetNormalAtDistance(distance);
                var rotation = Quaternion.LookRotation(Vector3.forward, direction);
                segment.transform.rotation = rotation;

                var spriteRenderer = segment.GetComponentInChildren<SpriteRenderer>();
                var color = spriteRenderer.color;

                color.a = CalculateTransparency(distance) * TRANSPARENCY_RATIO;
                spriteRenderer.color = color;

                delta += _deltaShift;           
            } 
          
            yield return null;
        }
    }

    private float CalculateTransparency(float distance)
    {
        var length = _pathCreator.path.length; ;
        var path = distance - (int)(distance / length) * length;
        return path / length * (length - path) / length;
    }
}
