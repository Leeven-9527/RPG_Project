using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Boss投掷石头对象池
public class RockPool : MonoSingleton<RockPool>
{
    
    public GameObject rockPreObj;//石头预制
    public int poolCount = 5; //池子初始大小
    public bool isLockPoolSize = false; //是否锁定池子大小

    private List<GameObject> poolObjList; //石头池链表
    private int currIndex = 0; //当前指向索引
    //  private System.Timers.Timer timeScheduler = new System.Timers.Timer(3000);//实例化Timer类，设置间隔时间为10000毫秒；
    

    void Start()
    {
        //初始化链表
        poolObjList = new List<GameObject>();
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = Instantiate(rockPreObj);
            obj.SetActive(false);
            poolObjList.Add(obj);
        }
    }

    public GameObject GetPooledObj()
    {
        for (int i = 0; i < poolObjList.Count; i++)
        {
            /*
             每一次遍历都是从上一次被使用的石头的下一个，而不是每次遍历从0开始。
            例如上一次获取了第4个石头，currentIndex就为5，这里从索引5开始遍历，这是一种贪心算法
            */
            int idx = (currIndex + i) % poolObjList.Count;
            //判断该石头是否在场景中激活。
            if (!poolObjList[idx].activeInHierarchy)
            {
                currIndex = (currIndex + 1) % poolObjList.Count;
                return poolObjList[idx];
            }
        }
        //如果遍历完一遍石头库发现没有可以用的，执行下面
        if (!isLockPoolSize) {
            //如果没有锁定对象池大小，创建石头并添加到对象池中。
            GameObject obj = Instantiate(rockPreObj);
            poolObjList.Add(obj);
            return obj;
        }
        //如果遍历完没有而且锁定了对象池大小，返回空。
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// 直接回收
    public void CollectObject(GameObject go)
    {
        go.SetActive(false);
    }


    /// 延迟回收
    public void CollectObject(GameObject go, float delay)
    {
        StartCoroutine(Collect(go, delay));
    }


    private IEnumerator Collect(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        CollectObject(go);
    }
}
