using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lesson_16 : MonoBehaviour
{


    private Canvas canvas;
    public Image rightHand;
    public Sprite[] handStateSprites;
    private PanelCenter panelCenter;
    private Lesson useKinectManager;

    public Image btn1;
    public Image btn2;
    public Image btn3;

    public Image circle1;
    public Image circle2;
    public Image circle3;
    public int gravityScale = 10;
    public int upForce = 2000;
    public RawImage kinectImg;

    public bool flag = true;
    private Image curBtn;//记录被切中的水果，希望知道这个水果什么时候掉落出界。出界的时候我们才执行切换界面。
    public int curBtnOutY = -300;//

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvasObj = GameObject.FindWithTag("Canvas");
        canvas = canvasObj.GetComponent<Canvas>();
        panelCenter = canvasObj.GetComponent<PanelCenter>();
        useKinectManager = canvasObj.GetComponent<Lesson>();
        useKinectManager.KinectImg = kinectImg;
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
                Vector3 handPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);
                Vector3 handScreenPosV3 = Camera.main.WorldToScreenPoint(handPos);

                Vector2 handsSenPos = new Vector2(handScreenPosV3.x, handScreenPosV3.y);
                Vector2 uguiPos;
                bool changeSuccess=RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, handScreenPosV3, Camera.main, out uguiPos);
                //

                if (changeSuccess)
                {
                    RectTransform rightRectTf = rightHand.transform as RectTransform;
                    rightRectTf.anchoredPosition = uguiPos;//
                }

                bool isHandClose = false;
                rightHand.sprite = handStateSprites[0];
                KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(userId);
                if (rightHandState == KinectInterop.HandState.Closed)
                {
                    //print("握拳");
                    isHandClose = true;
                    rightHand.sprite = handStateSprites[1];
                }

                if(isHandClose==true)
                {
                    //woquan
                    if (circle1.enabled == true && RectTransformUtility.RectangleContainsScreenPoint(circle1.rectTransform, handsSenPos, Camera.main))
                    {
                        //点击了第一个按钮
                        HandClickFruit(btn1);

                    }
                    else if (circle2.enabled == true && RectTransformUtility.RectangleContainsScreenPoint(circle2.rectTransform, handsSenPos, Camera.main))
                    {
                        //点击了第二个按钮
                        HandClickFruit(btn2);
                    }
                    else if (circle3.enabled == true && RectTransformUtility.RectangleContainsScreenPoint(circle3.rectTransform, handsSenPos, Camera.main))
                    {

                        //点击了第三个按钮
                        HandClickFruit(btn3);
                        
                    }
                }               


            }
        }

        deteCurBtn();
    }
    //执行检测什么时候水果出界
    private void deteCurBtn()
    {
        if (curBtn!=null)
        {
            RectTransform rtf = curBtn.transform as RectTransform;
            if (rtf.anchoredPosition.y < curBtnOutY)
            {
                //被切中的水果坐标出界了
                //切换界面
                panelCenter.showGamePanel();
                Destroy(gameObject);//销毁首页界面
            }
        }
    }

    //切到水果的处理
    private void HandClickFruit(Image clickFruit)
    {
        Rigidbody2D r1 = btn1.GetComponent<Rigidbody2D>();
        Rigidbody2D r2 = btn2.GetComponent<Rigidbody2D>();
        Rigidbody2D r3 = btn3.GetComponent<Rigidbody2D>();

        r1.gravityScale = gravityScale;
        r2.gravityScale = gravityScale;
        r3.gravityScale = gravityScale;

        circle1.enabled = false;
        circle2.enabled = false;
        circle3.enabled = false;

        curBtn = clickFruit;//记录了当前是那个水果被选择。接下来判断这个水果什么时候出界。

        if (clickFruit==btn1)
        {
            r1.AddForce(new Vector2(0, upForce));

        }
        else if (clickFruit==btn2)
        {
            r2.AddForce(new Vector2(0, upForce));
        }
        else
        {
            r3.AddForce(new Vector2(0, upForce));
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }

    }
}
