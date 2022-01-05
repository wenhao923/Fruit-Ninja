using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{
    public int type = 0;
    public Sprite[] fruitSprites;
    public GameObject particleObj;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //设置水果类型
    public void setType(int type)
    {
        this.type = type;
        image.sprite = fruitSprites[type];
        image.SetNativeSize();

        if (type==Contant.Type_Boom)
        {
            particleObj.SetActive(true);
        }
        else
        {
            particleObj.SetActive(false);
        }
    }
    //3D的碰撞器
    void OnTriggerEnter(Collider other)
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Bound")
        {
            //水果碰到了由Bound标示的周围
            if (type != Contant.Type_Apple && type != Contant.Type_Banana && type != Contant.Type_Basaha && type != Contant.Type_Boom && type != Contant.Type_Peach && type != Contant.Type_Sandia)
            {
                Destroy(gameObject);//只有是属于两半的水果切到的时候才销毁
            }
        }
    }


}
