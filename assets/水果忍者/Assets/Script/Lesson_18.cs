using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//游戏界面
public class Lesson_18 : MonoBehaviour
{
    public Fruit fruitClone;

    public int forceX = 300;
    public int forceY = 5000;
    public int forceHalfY = 3000;
    

    public Transform leftTrail;
    public Transform rightTrail;
    public RawImage KinectImg;
    public LifeContent lifeContent;
    public Text scoreText;
    public Image gameOverImg;

    private PanelCenter panelCenter;
    private const int minX = -286;
    private const int maxX = 286;
    private const int fruitInitY = -200;

    private const int fruitOutY = -290;

    private Fruit curFruit;

    private int lifeNum = 3;
    private int scoreNum = 0;

    void Start()
    {
        createFruit();
        gameOverImg.gameObject.SetActive(false);
        GameObject canvasObj = GameObject.FindWithTag("Canvas");
        //canvas = canvasObj.GetComponent<Canvas>();
        panelCenter = canvasObj.GetComponent<PanelCenter>();
    }

    // Update is called once per frame
    void Update()
    {
        bool needDestoryFruit = false;
        bool isOutDestroy = false;//是否是出界而被消除的

        if (KinectImg.texture == null)
        {
            //Texture2D kinectpic = KinectManager.Instance.GetUsersClrTex();
            Texture2D kinectUseMap = KinectManager.Instance.GetUsersLblTex();

            KinectImg.texture = kinectUseMap;
        }


        if (curFruit != null)
        {
            if (KinectManager.Instance.IsUserDetected())
            {
                long userId = KinectManager.Instance.GetPrimaryUserID();

                int jointType = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(userId, jointType))
                {
                    Vector3 handPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);

                    rightTrail.position = handPos;

                    Vector3 handScreenPosV3 = Camera.main.WorldToScreenPoint(handPos);
                    Vector2 handsSenPos = new Vector2(handScreenPosV3.x, handScreenPosV3.y);
                    //
                    KinectInterop.HandState rightHandState= KinectManager.Instance.GetRightHandState(userId);

                    if (rightHandState==KinectInterop.HandState.Open&& RectTransformUtility.RectangleContainsScreenPoint(curFruit.transform as RectTransform, handsSenPos, Camera.main))
                    {
                        //手掌是张开状态并且右手切中水果了
                        needDestoryFruit = true;
                    }

                }//右手逻辑

                jointType = (int)KinectInterop.JointType.HandLeft;//表示左手
                if (KinectManager.Instance.IsJointTracked(userId, jointType))
                {
                    Vector3 handPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);//左手在Kinect中的三维坐标

                    leftTrail.position = handPos;

                    Vector3 handScreenPosV3 = Camera.main.WorldToScreenPoint(handPos);
                    Vector2 handsSenPos = new Vector2(handScreenPosV3.x, handScreenPosV3.y);
                    //
                    KinectInterop.HandState leftHandState = KinectManager.Instance.GetLeftHandState(userId);

                    if (leftHandState== KinectInterop.HandState.Open&& RectTransformUtility.RectangleContainsScreenPoint(curFruit.transform as RectTransform, handsSenPos, Camera.main))
                    {
                        //右手切中水果了
                        needDestoryFruit = true;
                    }

                }//左手逻辑
            }

            RectTransform rtf = curFruit.transform as RectTransform;
            float curFruitY = rtf.anchoredPosition.y;//每一帧获取当前水果y值
            if (curFruitY < fruitOutY)
            {
                needDestoryFruit = true;
                isOutDestroy = true;
                //水果跑出屏幕外，销毁
            }

            


            if (needDestoryFruit)
            {
                //切中或者出界
                if (isOutDestroy==true)
                {
                    //出界
                    if (curFruit.type==Contant.Type_Boom)
                    {
                        //炸弹出界，不扣分
                        scoreNum++;
                    }
                    else
                    {
                        //水果出界，扣分
                        scoreNum--;
                    }
                }
                else
                {
                    //切中
                    if (curFruit.type == Contant.Type_Boom)
                    {
                        //切中炸弹，扣生命值
                        lifeNum--;
                    }
                    else
                    {
                        //切中水果，加分
                        scoreNum++;
                    }
                }
            }


            lifeContent.setLife(lifeNum);
            scoreText.text = scoreNum+"";

            if (needDestoryFruit)
            {
                //销毁

                if (isOutDestroy == false)
                {
                    //水果因为被切中才需要消除的
                    if (curFruit.type != Contant.Type_Boom)
                    {
                        //切刀的不是炸弹才生成左右水果
                        createLeftRightFruit();

                    }
                }


                Destroy(curFruit.gameObject);

                if (lifeNum>0)
                {
                    createFruit();//再生成一个新水果
                }
                else
                {
                    //没有生命值了
                    gameOverImg.gameObject.SetActive(true);
                }

            }


        }

        detectedClickGameOver();
    }

    //检查是否点击游戏结束按钮
    private void detectedClickGameOver()
    {
        if (lifeNum>0)
        {
            return;
        }
        if (KinectManager.Instance.IsUserDetected())
        {
            long userId = KinectManager.Instance.GetPrimaryUserID();

            int jointType = (int)KinectInterop.JointType.HandRight;
            if (KinectManager.Instance.IsJointTracked(userId, jointType))
            {
                Vector3 handPos = KinectManager.Instance.GetJointKinectPosition(userId, jointType);

                rightTrail.position = handPos;

                Vector3 handScreenPosV3 = Camera.main.WorldToScreenPoint(handPos);
                Vector2 handsSenPos = new Vector2(handScreenPosV3.x, handScreenPosV3.y);
                //
                KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(userId);

                if (rightHandState == KinectInterop.HandState.Lasso && RectTransformUtility.RectangleContainsScreenPoint(gameOverImg.transform as RectTransform, handsSenPos, Camera.main))
                {
             
                    //手掌是张开状态并且右手切中水果了
                    //点击了游戏结束按钮。重新开始游戏
                    Destroy(gameObject);
                    panelCenter.showFirstPanel();
                }

            }//右手逻辑
        }
    }
    //创建左右两边水果
    public void createLeftRightFruit(   )
    {
        //以当前水果为模板克隆左右两个水果
        Fruit leftFruit = Instantiate(curFruit, curFruit.transform.position, curFruit.transform.rotation) as Fruit;
        leftFruit.setType(curFruit.type+1);
        newFruitInit(leftFruit,true);

        Fruit rightFruit = Instantiate(curFruit, curFruit.transform.position, curFruit.transform.rotation) as Fruit;
        rightFruit.setType(curFruit.type + 2);
        newFruitInit(rightFruit,false);
    }

    private void newFruitInit(Fruit fruit,bool isLeft)
    {
        RectTransform curRtf= curFruit.transform as RectTransform;
        fruit.transform.SetParent(transform);
        RectTransform rtf = fruit.transform as RectTransform;
        rtf.anchoredPosition3D = new Vector3(0, 0, 0);
        rtf.anchoredPosition = curRtf.anchoredPosition;
        rtf.localScale = new Vector3(1, 1, 1);

        Rigidbody2D rb2d= fruit.GetComponent<Rigidbody2D>();
        if (isLeft)
        {
            //左边一半水果
            rb2d.AddForce(new Vector2(-forceX, forceHalfY));
        }
        else
        {
            rb2d.AddForce(new Vector2(forceX, forceHalfY));
        }
    }

    private void createFruit()
    {
        curFruit = Instantiate(fruitClone) as Fruit;//调用预制
        curFruit.transform.SetParent(transform);
        RectTransform fruitRt = curFruit.transform as RectTransform;
        fruitRt.anchoredPosition3D = new Vector3(0, 0, 0);
        int fruitX = Random.Range(minX, maxX);
        fruitRt.anchoredPosition = new Vector2(fruitX, fruitInitY);//水果有了随机位置
        fruitRt.localScale = new Vector3(1, 1, 1);
        int[] types ={Contant.Type_Boom,Contant.Type_Apple,Contant.Type_Banana,Contant.Type_Basaha,Contant.Type_Peach,Contant.Type_Sandia};
        int typesIndex = Random.Range(0, 5);

        int fruitType = types[typesIndex];

        curFruit.setType(fruitType);
        Rigidbody2D rigid2d = curFruit.GetComponent<Rigidbody2D>();
        if (fruitX > 0)
        {

            rigid2d.AddForce(new Vector2(-forceX, forceY));//由水果的位置决定力的方向
        }
        else
        {

            rigid2d.AddForce(new Vector2(forceX, forceY));
        }
    }
}
