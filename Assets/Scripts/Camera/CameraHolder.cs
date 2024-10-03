using Levels;
using UnityEngine;
using Utils.Singleton;

namespace CameraControl
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private Camera mainCamera;

        public Camera MainCamera => mainCamera;       
    }
}