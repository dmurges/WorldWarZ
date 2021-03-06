﻿
using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;        //an Object to lock on to
    public float damping = 6.0f;    //to control the rotation 
    public bool smooth = true;
    public float minDistance = 10.0f;    //How far the target is from the camera
    public string property = "";
    private Vector3 offset;

    private Color color;
    private float alpha = 1.0f;
    private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {
        if (target)
        {
            _myTransform.position = target.position - offset;
            if (smooth)
            {

                //Look at and dampen the rotation
                Quaternion rotation = Quaternion.LookRotation(target.position - _myTransform.position);
                _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            { //Just look at

                _myTransform.rotation = Quaternion.FromToRotation(-Vector3.forward, (new Vector3(target.position.x, target.position.y, target.position.z) - _myTransform.position).normalized);

                float distance = Vector3.Distance(target.position, _myTransform.position);

                if (distance < minDistance)
                {
                    alpha = Mathf.Lerp(alpha, 0.0f, Time.deltaTime * 2.0f);
                }
                else
                {
                    alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime * 2.0f);

                }
            }
        }
    }
}