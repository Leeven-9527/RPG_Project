using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

//public class GameManager
//{
//    private static GameManager instance;    //单例对象，全局唯一

//    public static GameManager GetInstance   //获取接口,也可以写成函数形式
//    {
//        get
//        {
//            if (instance == null)
//                instance = Activator.CreateInstance<GameManager>();
//            return instance;
//        }
//    }

//    public void Init()
//    {
//        Debug.Log("Initialized");
//    }
//}
///// <summary>
///// 调用方式
///// </summary>
////GameManager.GetInstance.Init();

//DontDestroyOnLoad场景常驻单例
//如果不希望切换场景时单例被销毁需要使用Unity的 DontDestroyOnLoad(GameObject obj) 函数
//public class BoxCtrl : MonoBehaviour
//{
//    private static BoxCtrl _Instance;

//    public static BoxCtrl Instance
//    {
//        get
//        {
//            if (_Instance == null)
//            {
//                //创建一个新的物体
//                GameObject obj = new GameObject("BoxCtrl");
//                //将单例挂载在物体上
//                _Instance = obj.AddComponent<BoxCtrl>();
//                //使得加载场景时候，物体不会被摧毁
//                DontDestroyOnLoad(obj);
//            }
//            return _Instance;
//        }
//    }
//    public void Test()
//    {
//        Debug.Log("执行BoxCtrl单例");
//    }
//}

public class MouseManager : MonoSingleton<MouseManager>
{
    //public static MouseManager Instance;    //单例对象，全局唯一
    //public EventVector3 OnMouseClick;

    public event Action<Vector3> OnMouseClick; //鼠标点击移动事件
    RaycastHit hitInfo;
    public Texture2D point, doorway, attack, target, arrow;
    public event Action<GameObject> OnEnemyClick; //鼠标点击敌人事件


    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }


    void SetCurrsorTexture()
    {
        Vector3 mousePoint = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePoint);
        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(point, new Vector2(32, 32), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(target, new Vector2(32, 32), CursorMode.Auto);
                    break;
            }

        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }

    void Update()
    {
        SetCurrsorTexture();
        MouseControl();
    }


}
