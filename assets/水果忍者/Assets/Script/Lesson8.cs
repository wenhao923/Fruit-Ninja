using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lesson8 : MonoBehaviour
{
    public Canvas canvas;
    public Image rightHand;
    public Image btn1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (KinectManager.Instance.IsUserDetected())
        {
            long userId = KinectManager.Instance.GetPrimaryUserID();

            int jointType = (int)KinectInterop.JointType.HandRight;
            if (KinectManager.Instance.IsJointTracked(userId, jointType))
            {
                Vector3 leftHandPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);
                Vector3 rightHandScreenPos = Camera.main.WorldToScreenPoint(leftHandPos);
                Vector2 rightHandsSenPos = new Vector2(rightHandScreenPos.x, rightHandScreenPos.y);
                Vector2 rightHandUguiPos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, rightHandScreenPos, null, out rightHandUguiPos))
                {
                    RectTransform rightRectTf = rightHand.transform as RectTransform;
                    rightRectTf.anchoredPosition = rightHandUguiPos;
                }
                if (RectTransformUtility.RectangleContainsScreenPoint(btn1.rectTransform, rightHandsSenPos, null))
                {
                    print("手在按钮上悬停");
                    //手在按钮一悬停
                    KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(userId);
                    if(rightHandState==KinectInterop.HandState.Closed)
                    {
                        print("握拳");
                    }
                }
                else
                {
                    print("手离开");
                }



            }
        }
    }
}
