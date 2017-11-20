using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    public Color color;

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
