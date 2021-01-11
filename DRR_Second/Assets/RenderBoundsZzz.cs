using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBoundsZzz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDrawGizmosSelected() {
        // A sphere that fully encloses the bounding box.
        Vector3 center = this.GetComponent<Renderer>().bounds.center;
        float radius = this.GetComponent<Renderer>().bounds.extents.magnitude;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(center, radius);
    }
}
