using System.Collections.Generic;
using UnityEngine;

public class ResponsiveSelector : MonoBehaviour, ISelector
{
    [SerializeField] private List<Selectable> selectables;
    [SerializeField] private float threshold = 0.97f;
    
    private Transform _selection;

    public void Check(Ray ray)
    {
        _selection = null;

        var closest = 0f;
        
        for (int i = 0; i < selectables.Count; i++)
        {
            var vector1 = ray.direction;
            var vector2 = selectables[i].transform.position - ray.origin;
        
            var lookPercentage = Vector3.Dot(vector1.normalized, vector2.normalized);

            selectables[i].LookPercentage = lookPercentage;

            if (lookPercentage > threshold && lookPercentage > closest)
            {
                closest = lookPercentage;
                _selection = selectables[i].transform;
            }
        }
    }

    public Transform GetSelection()
    {
        return _selection;
    }
}