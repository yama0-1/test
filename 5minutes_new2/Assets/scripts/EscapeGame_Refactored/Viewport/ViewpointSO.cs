using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EscapeGame/Viewpoint")]

public class ViewpointSO : ScriptableObject
{
    [SerializeField] private Vector3 _cameraPosition;
    [SerializeField] private Vector3 _cameraEulerAngles;

    public Vector3 CameraPosition => _cameraPosition;
    public Vector3 CameraEulerAngles => _cameraEulerAngles;
}
