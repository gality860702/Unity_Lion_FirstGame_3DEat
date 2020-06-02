using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    #region 欄位與屬性
    [Header("移動速度"),Range(1,100)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 500)]
    public float height;


    ///<sunnmary>
    ///是否在地板上
    /// </sunnmary>
    private bool isGround
    {
        get
        {
            if (transform.position.y < 0.88f) return true;  //如果 y軸 小於 0.051 傳回true
            else return false;                               //否則 傳回 false
        }
    }
    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 angle;

    private Animator ani;
    private Rigidbody rig;
    private AudioSource aud;   //喇叭
    private GameManager gm;    //遊戲管理器

    /// <summary>
    /// 跳躍力道:從0 慢慢增加
    /// </summary>
    private float jump;
    #endregion

    #region 方法
    /// <summary>
    /// 移動:透過鍵盤
    /// </summary>
    private void Move()
    {
        #region 移動:透過鍵盤
        //浮點數 前後值 =輸入類別,取得軸向值("垂直")、垂直 WS上下
        float v = Input.GetAxisRaw("Vertical");
        //水平ad左右
        float h = Input.GetAxisRaw("Horizontal");

        //剛體物件的添加推力(x,y,z)
        //rig.AddForce(0, 0, speed * v);
        //剛體,添加推力(三維向量)
        //前方 tarnsform.forward -z
        //右方 transform.right   -x
        //上方 transform.up      -y
        rig.AddForce(transform.forward * speed * Mathf.Abs(v));
        rig.AddForce(transform.forward * speed * Mathf.Abs(h));

        //動畫,設定布林值("跑步參數",布林值)-當 前後取絕對值 大於0時勾選
        ani.SetBool("跑步開關", Mathf.Abs(v) > 0);
        //ani.SetBool("跑步開關", v == 1 || v==-1);//使用邏輯運算子
        #endregion



        #region 轉向:

        if (v == 1) angle = new Vector3(0, 0, 0);                   //前 y 0
        else if (v == -1) angle = new Vector3(0, 180, 0);           //後 y 180
        else if (h == 1) angle = new Vector3(0, 90, 0);             //右 y 90
        else if (h == -1) angle = new Vector3(0, 270, 0);           //左 y 270
        // 只要類別後面有: MonoBehaviour
        // 就可以直接使用關鍵字 transform 取得此物件的transform 元件
        // eluerAngles 歐拉角度 0-360
        transform.eulerAngles = angle;
        //print("角度:" + angle);
        #endregion
    }
    #region
    /// <summary>
    /// 跳躍:判斷在地板上按下空白鍵跳躍
    /// </summary>
    private void Jump()
    {
        //如果 在地板上 為 勾選 並且 按下空白鍵
        if(isGround && Input.GetButtonDown("Jump"))
        {
            //每次跳躍,值從0開始
            jump = 0;
            //剛體,推力(0,跳躍高度,0)
            rig.AddForce(0, height, 0);
        }
        //如果 不在地板上(在空中)
        if (!isGround)
        {
            //跳躍 遞增 時間,一禎時間
            jump += Time.deltaTime*3;
        }
        //動畫,設定浮點數("跳躍參數",跳躍時間)
        ani.SetFloat("跳躍力道", jump);
    }

    [Header("薄餅音效")]
    public AudioClip soundbin;
    [Header("毒蘑菇音效")]
    public AudioClip soundmush;


    /// <summary>
    /// 碰到道具:碰到帶有標籤[薄餅]的物件
    /// </summary>
    /// <param name="prop"></param>
    private void Hitprop(GameObject prop)
    {
        if (prop.tag == "薄餅")
        {
            aud.PlayOneShot(soundbin, 2);
            Destroy(prop);
        }
        else if (prop.tag == "毒蘑菇")
        {
            aud.PlayOneShot(soundmush, 2);
            Destroy(prop);

           
        }
        gm.GetProp(prop.tag);                //告知 GM 取得道具(將道具和標籤傳過去)
    }

    #endregion 

    #region 事件
    private void Start()
    {
        //GetComponent<泛型>() 泛型方法 -泛型 所有類型 Rigidbody,Transform,Collider..
        //剛體=取得元件<剛體>();
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        //fOOT 僅限於場景上只有一個類別存在時使用
        //例如:場景上只有一個 GameManager 類別時可以用它來取得
        gm = FindObjectOfType<GameManager>();
}

    //固定更新頻率事件:1秒50禎,使用物理必須再次事件內
    private void FixedUpdate()
    {
        Move();
    }
    //更新事件1秒約60禎
    private void Update()
    {
        Jump();       
    }

    //碰撞事件:當物件碰撞開始時執行一次(沒有勾選 Is Trigger)
    //collision 碰到物件的碰撞資訊
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    //碰撞事件:當物件碰撞離開時執行一次(沒有勾選 Is Trigger)
    private void OnCollisionExit(Collision collision)
    {
        
    }
    //當物件碰撞開始時持續執行(沒勾選Is Trigger)
    private void OnCollisionStay(Collision collision)
    {
        
    }
    /*---*/
    //觸發事件:當物件碰撞開始時執行一次
    private void OnTriggerEnter(Collider other)
    {
        //碰到道具(碰撞資訊,遊戲物件)
        Hitprop(other.gameObject);
    }



    //觸發事件:當物件碰撞開始時執行一次(有勾(Is Trigger)
    private void OnTriggerExit(Collider other)
    {
        
    }
    //觸發事件:當物件碰撞開始時持續執行(有勾選Is Trigger) @ FPS
    private void OnTriggerStay(Collider other)
    {

}
    #endregion

 #endregion  
}

