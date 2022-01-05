using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson10 : MonoBehaviour,KinectGestures.GestureListenerInterface
{

    private Text text;

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return true;
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if(gesture==KinectGestures.Gestures.Push)
        {
            text.text = "用户做了SwipeRight手势";
        }
        return true;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        
    }

    public void UserDetected(long userId, int userIndex)
    {
        if (text != null)
        {

            text.text = "检测到用户";
        }
    }

    public void UserLost(long userId, int userIndex)
    {
        if (text != null)
        {

            text.text = "用户离开摄像头";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
