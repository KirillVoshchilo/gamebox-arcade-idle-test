using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasStorage : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    public Camera MainCamera 
        => _mainCamera; 
}
