using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson : MonoBehaviour
{
    public RawImage KinectImg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isInit = KinectManager.Instance.IsInitialized();
        if (isInit)
        {
            if (KinectImg!=null &&KinectImg.texture == null)
            {
                //Texture2D kinectpic = KinectManager.Instance.GetUsersClrTex();
                Texture2D kinectUseMap = KinectManager.Instance.GetUsersLblTex();

                KinectImg.texture = kinectUseMap;
            }
            if (KinectManager.Instance.IsUserDetected())
            {
                long userId = KinectManager.Instance.GetPrimaryUserID();

                int jointType = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(userId, jointType))
                {
                    Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);
                    KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(userId);
                    if (rightHandState == KinectInterop.HandState.Closed)
                    {
                        //握拳
                    }
                    else if (rightHandState == KinectInterop.HandState.Open)
                    {
                        //右手松手
                    }
                    else if (rightHandState == KinectInterop.HandState.Lasso)
                    {
                        //yes手势
                    }
                }
            }


        }
    }
}
