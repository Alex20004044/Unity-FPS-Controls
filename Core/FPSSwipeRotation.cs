using MSFD.AS;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace HS
{
    public class FPSSwipeRotation : MonoBehaviour
    {
        [Header("Vert up boundary")]
        [SerializeField]
        float minXRotation = 90;
        [Header("Vert down boundary")]
        [SerializeField]
        float maxXRotation = 270;
        [SerializeField]
        bool isInvert = false;

        [SerializeField]
        float sensitivity = 100f;
        [SerializeField]
        float _swipeTreshold = 100f;
        [SerializeField]
        bool isAllowDryRunWhenStartSwiping = true;
        [SerializeField]
        bool isScaleSensitivityWithScreenSize = true;

        [SerializeField]
        Transform head;

        Transform vertTransform;
        Transform horTransform;

        bool isSwipe = false;
        Vector2 targetRotation = Vector2.zero;
        Vector2 baseRotation = Vector2.zero;

        Vector2 startSwipeCoord = Vector2.zero;

        float ScreenFactor
        {
            get
            {
                if (!isScaleSensitivityWithScreenSize)
                    return 1;
                // Get shortest size
                var size = Mathf.Min(Screen.width, Screen.height);

                // If it's 0 or less, it's invalid, so return the default scale of 1.0
                if (size <= 0)
                {
                    return 1.0f;
                }

                // Return reciprocal for easy multiplication
                return 1.0f / size;
            }
        }

        float SwipeTreshold { get => _swipeTreshold * ScreenFactor; }

        private void Start()
        {
            vertTransform = head;
            horTransform = transform;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startSwipeCoord = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isSwipe = false;
            }
            if (Input.GetMouseButton(0) && !isSwipe &&
                Mathf.Abs((Input.mousePosition.ConvertVector3ToVector2(Coordinates.ConvertV3ToV2Mode.y_to_y) - startSwipeCoord).magnitude) > SwipeTreshold)
            {
                isSwipe = true;
                baseRotation = new Vector2(vertTransform.localEulerAngles.x, horTransform.localEulerAngles.y);
                if (isAllowDryRunWhenStartSwiping)
                    startSwipeCoord = Input.mousePosition.ConvertVector3ToVector2(Coordinates.ConvertV3ToV2Mode.y_to_y);
            }
            if (isSwipe)
            {
                targetRotation.y = baseRotation.y + GetYRot() * sensitivity * ScreenFactor;

                targetRotation.x = AlignAngle(baseRotation.x) + GetXRot() * sensitivity * ScreenFactor;
                targetRotation.x = Mathf.Clamp(targetRotation.x, minXRotation, maxXRotation);
                
                //Instant rotate. Can be changed to more smooth rotation
                Vector3 rot = vertTransform.localEulerAngles;
                vertTransform.localEulerAngles = new Vector3(targetRotation.x, rot.y, rot.z);
                rot = horTransform.localEulerAngles;
                horTransform.localEulerAngles = new Vector3(rot.x, targetRotation.y, rot.z);
            }
        }
        /// <summary>
        /// Convert angle to inspector representation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float AlignAngle(float value)
        {
            float angle = value - 180;
            if (angle > 0)
                return angle - 180;
            return angle + 180;
        }
        float GetYRot()
        {
            return Input.mousePosition.x - startSwipeCoord.x;
        }

        float GetXRot()
        {
            return -(Input.mousePosition.y - startSwipeCoord.y) * (isInvert ? -1f : 1f);
        }
    }
}
