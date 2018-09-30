using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitonLib.Pattern;

/// <summary>
/// 万有引力模拟系统，引力是始终指向一个球体中心的，相当于重力中心
/// 不是Unity默认的始终朝下的重力方向
/// </summary>
public class PointGravitySystem : SceneSingleton<PointGravitySystem>
{
    /// <summary>
    /// 万有引力适配乘数，用来放大万有引力
    /// </summary>
    [SerializeField, Tooltip("万有引力适配乘数，用来放大万有引力")]
    private float _gravityAccelar;
    [SerializeField,Range(-1,-11),Tooltip("万有引力常量次方值，用于计算万有引力常量，\n值越大引力越大")]
    private int _gPow = -5;
    private float G;
    /// <summary>
    /// 受重力影响的物体
    /// </summary>
    private static List<Rigidbody> _massObjList = new List<Rigidbody>();
    /// <summary>
    /// 重力中心
    /// </summary>
    private Rigidbody[] _gravityCenter;

    //测试用
    public Rigidbody gravityCenter;
    protected override void Awake()
    {
        base.Awake();
        InitSystem();
    }

    void Start()
    {

    }


    void FixedUpdate()
    {
        for (int i = 0; i < _massObjList.Count; ++i)
        {
            Rigidbody obj = _massObjList[i];
            Vector3 toCenter = gravityCenter.transform.position - obj.transform.position;
            toCenter = toCenter.normalized * ((G * gravityCenter.mass * obj.mass) / toCenter.sqrMagnitude);
            obj.AddForce(toCenter*_gravityAccelar );
        }
    }
    /// <summary>
    /// 初始化系统设置
    /// </summary>
    private void InitSystem()
    {
        G = 6.67f * Mathf.Pow(10, _gPow);
    }
    /// <summary>
    /// 添加到重力环境里
    /// </summary>
    /// <param name="obj">需要受中心点重力影响的物体</param>
    public static void AddToSystem(Rigidbody obj)
    {
        if (_massObjList.Contains(obj)) return;
        _massObjList.Add(obj);
    }
    /// <summary>
    /// 给物体移除中心重力
    /// </summary>
    /// <param name="obj"></param>
    public static void RemoveMassObj(Rigidbody obj)
    {
        if (_massObjList.Contains(obj))
            _massObjList.Remove(obj);
    }
}

