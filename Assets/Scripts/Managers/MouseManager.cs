using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

//public class GameManager
//{
//    private static GameManager instance;    //��������ȫ��Ψһ

//    public static GameManager GetInstance   //��ȡ�ӿ�,Ҳ����д�ɺ�����ʽ
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
///// ���÷�ʽ
///// </summary>
////GameManager.GetInstance.Init();

//DontDestroyOnLoad������פ����
//�����ϣ���л�����ʱ������������Ҫʹ��Unity�� DontDestroyOnLoad(GameObject obj) ����
//public class BoxCtrl : MonoBehaviour
//{
//    private static BoxCtrl _Instance;

//    public static BoxCtrl Instance
//    {
//        get
//        {
//            if (_Instance == null)
//            {
//                //����һ���µ�����
//                GameObject obj = new GameObject("BoxCtrl");
//                //������������������
//                _Instance = obj.AddComponent<BoxCtrl>();
//                //ʹ�ü��س���ʱ�����岻�ᱻ�ݻ�
//                DontDestroyOnLoad(obj);
//            }
//            return _Instance;
//        }
//    }
//    public void Test()
//    {
//        Debug.Log("ִ��BoxCtrl����");
//    }
//}

public class MouseManager : MonoSingleton<MouseManager>
{
    //public static MouseManager Instance;    //��������ȫ��Ψһ
    //public EventVector3 OnMouseClick;

    public event Action<Vector3> OnMouseClick; //������ƶ��¼�
    RaycastHit hitInfo;
    public Texture2D point, doorway, attack, target, arrow;
    public event Action<GameObject> OnEnemyClick; //����������¼�


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
            //�л������ͼ
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
