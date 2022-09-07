using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CharacterStats playerStats;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /*
     * 观察者模式反向注册的方法让 
     * player在生成的时候告诉gamemanager
     * 我是player的CharacterStats
     */
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    // 添加到观察列表
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    // 从观察列表移除
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    //广播
    public void NotifyObserver(){
        for (int i = 0; i < endGameObservers.Count; i++)
        {
            endGameObservers[i].EndNotify();
        }
    }
}
