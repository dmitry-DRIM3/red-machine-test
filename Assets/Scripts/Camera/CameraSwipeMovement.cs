using Levels;
using Player;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using Utils.Singleton;

namespace CameraControl
{
    public class CameraSwipeMovement : DontDestroyMonoBehaviour
    {
        [SerializeField] private float swipeSpeed;
        [SerializeField] private float smoothSpeed;

        private LevelSettings levelSettings;
        private Vector3 touchStart;
        private Camera mainCamera;

        private void Start()
        {
            this.mainCamera = CameraHolder.Instance.MainCamera;
            LevelsManager.Instance.LevelSettingsChanged += SetLevelSettings;
        }

        private void Update()
        {
            MovementCamera();
        }

        private void SetLevelSettings(LevelSettings levelSettings)
        {
            this.levelSettings = levelSettings;
        }

        private void MovementCamera()
        {
            if (!levelSettings) return;

            Vector3 currentPosition;

#if UNITY_ANDROID || UNITY_IOS      
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                currentPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
                HandleTouchMovement(touch, currentPosition);
            }
#else
            if (Input.GetMouseButton(0))
            {
                currentPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
                HandleMouseMovement(currentPosition);
            }
#endif
        }

        private void HandleTouchMovement(Touch touch, Vector3 currentTouchPosition)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchStart = currentTouchPosition;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                MoveCamera(currentTouchPosition);
            }
        }

        private void HandleMouseMovement(Vector3 currentMousePosition)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = currentMousePosition;
            }
            MoveCamera(currentMousePosition);
        }

        private void MoveCamera(Vector3 currentPosition)
        {
            if (PlayerController.PlayerState == PlayerState.Connecting) return;

            Vector3 direction = touchStart - currentPosition;

            Vector3 targetPosition = mainCamera.transform.position + direction * swipeSpeed * Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, smoothSpeed);

            mainCamera.transform.position = new Vector3(
                Mathf.Clamp(mainCamera.transform.position.x, levelSettings.minX, levelSettings.maxX),
                Mathf.Clamp(mainCamera.transform.position.y, levelSettings.minY, levelSettings.maxY),
                mainCamera.transform.position.z
            );

            touchStart = currentPosition;
        }  
    }
}
