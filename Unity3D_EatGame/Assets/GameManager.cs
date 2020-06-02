using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                     // UI API
using UnityEngine.SceneManagement;        // 場景管理 API

public class GameManager : MonoBehaviour
{
    #region 欄位與屬性
    [Header("道具")]
    public GameObject[] props;
    [Header("文字介面:道具總數")]
    public Text textCount;
    [Header("文字介面:時間")]
    public Text textTime;
    [Header("文字介面:結束畫面標題")]
    public Text textTitle;
    [Header("結束畫面")]
    public CanvasGroup final;

    ///<summary>
    ///道具總數
    /// </summary>
    private int countTotal;

    ///<summary>
    ///取得道具數量
    /// </summary>
    private int countProp;
    #endregion

    ///<summary>
    ///遊戲時間
    /// </summary>
    private float gameTime = 30;

    #region 方法
    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop">想要生成的道具</param>
    /// <param name="count">想要生成的數量+隨機值 + - 5 </param>
    /// //<returns>傳回生成幾顆</returns>
    private int CreateProp(GameObject prop, int count)
    {
        //取得隨機道具數量=指定的數量 + - 5
        int total = count + Random.Range(-2, 2);

        //for迴圈
        for(int i=0;i<total;i++)
        {
            //座標 = (隨機.1.5.隨機)
            Vector3 pos = new Vector3(Random.Range(-9, 9), 1.5f, Random.Range(-9, 9));
            //生成(物件,座標,角度)
            Instantiate(prop, pos, Quaternion.identity);
        }
        //傳回 道具數量
        return total;
        }
    

    /// <summary>
    /// 時間倒數
    /// </summary>
    private void CountTime()
    {
        // 如果取得所有雞腿 就 跳出
        if (countProp == countTotal) return;

        //遊戲時間 遞減 一禎的時間
        gameTime -= Time.deltaTime;

        //遊戲時間=數學,夾住(遊戲時間、最小值、最大值)
        gameTime = Mathf.Clamp(gameTime, 0, 100);

        //更新倒數時間介面 ToString("f小數點位數")
        textTime.text = "倒數時間:" + gameTime.ToString("f2");

        Lose();
    }

    /// <summary>
    /// 取得道具:薄餅 - 更新數量與介面、毒蘑菇 - 扣兩秒並更新介面
    /// </summary>
    /// <param name="prop">道具名稱</param>
    public void GetProp(string prop)
    {
        if (prop == "薄餅")
        {
            countProp++;
            textCount.text = "道具數量:" + countProp + "/" + countTotal;

            Win();             // 呼叫勝利方法
        }

        else if (prop == "毒蘑菇")
        {
            gameTime -= 2;
            textTime.text = "倒數時間:" + gameTime.ToString("f2");
        
        }
    }

    /// <summary>
    /// 勝利:吃光所有薄餅
    /// </summary>
    private void Win()
    {
        if (countProp == countTotal)                     //如果雞腿數量=雞腿總數
        {
            final.alpha = 1;                             //顯示結束畫面
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "恭喜你吃完所有雞腿";
        }
    }

   ///<summary>
   ///失敗:時間結束
   /// </summary>
   private void Lose()
    {
        if (gameTime == 0)
        {
            final.alpha = 1;
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "挑戰失敗!!";
            FindObjectOfType<player>().enabled = false;          //取得玩家,啟動 =false
             
        }
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene("遊戲選單");
    }



    ///<summary>
    ///離開遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit();  //應用程式,離開()
    }
    #endregion

    #region 事件
    private void Start()
    {
        countTotal = CreateProp(props[0], 7); //道具總數=生成道具(道具一號,指定數量)

        textCount.text = "道具數量:0/" + countTotal;

        CreateProp(props[1], 3);              //生成道具(道具二號,指定數量)
    }


    private void Update()
    {
        CountTime();
    }
    #endregion
    
}
