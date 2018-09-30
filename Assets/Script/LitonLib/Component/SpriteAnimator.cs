using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LitonLib.CustomComponent
{

   // [RequireComponent(typeof(SpriteRenderer))]
    [AddComponentMenu("Liton/Animation/SpriteAnimator")]
    public class SpriteAnimator : MonoBehaviour
    {
        public SpriteAnimatorConfig m_config;
        public bool PlayAutomaticly;
        [SerializeField, Tooltip("精灵序列帧")]
        private Sprite[] _spritesArray;
        private bool _isPlaying = false;
        //当前帧
        private int _currentFrame = -1;
        //总帧数
        private int _frameLength = -1;
        //上次刷新时间
        private float _lastFreshTime = 0f;
        private Image _render;

        /// <summary>
        /// 动画播放速度
        /// </summary>
        public float Speed
        {
            get
            {
                return m_config.m_speed;
            }
            set
            {
                m_config.m_speed = Mathf.Max(0, value);
            }
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }




        void Awake()
        {
            InitAnimator();
        }
        void Start()
        {

        }
        void Update()
        {
            if (_isPlaying)
            {
                UpdateFrame();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        void InitAnimator()
        {
            if (PlayAutomaticly) _isPlaying = true;
            _frameLength = _spritesArray == null ? 0 : _spritesArray.Length;
            _render = GetComponent<Image>();
            if (_render == null)
            {
                Debug.LogErrorFormat("{0} 没有SpriteRender组件", gameObject.name);
            }
        }
        /// <summary>
        /// 定时更新序列帧
        /// </summary>
        private void UpdateFrame()
        {
            if (_spritesArray == null || _spritesArray.Length == 0) return;
            if (Time.time - _lastFreshTime > m_config.m_frameDelta / m_config.m_speed)
            {
                _currentFrame = NextFrameOrder(!m_config.m_playReverse, _currentFrame);
                _render.sprite = _spritesArray[_currentFrame];
                _lastFreshTime = Time.time;
            }
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="forward"></param>
        public void Play(bool forward = true)
        {
            _isPlaying = true;
            m_config.m_playReverse = !forward;
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _isPlaying = false;
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            _isPlaying = false;
            ResetAnimationFrame();
        }
        /// <summary>
        /// 重置序列帧
        /// </summary>
        private void ResetAnimationFrame()
        {
            _currentFrame = 0;
        }
        /// <summary>
        /// 获取下一张图片
        /// </summary>
        /// <param name="playForward">是否向前播放</param>
        /// <param name="currentFrame"></param>
        /// <returns></returns>
        private int NextFrameOrder(bool playForward, int currentFrame)
        {
            if (playForward)
                currentFrame = currentFrame + 1 == _frameLength ? 0 : currentFrame + 1;
            else
                currentFrame = currentFrame - 1 < 0 ? _frameLength - 1 : currentFrame - 1;
            return currentFrame;
        }
    }
}
[System.Serializable]
/// <summary>
/// 精灵动画状态机配置类
/// </summary>
public class SpriteAnimatorConfig
{
    [Tooltip("反向播放")]
    public bool m_playReverse;
    [Tooltip("播放速率")]
    public float m_speed;
    [Tooltip("每帧时间间隔")]
    public float m_frameDelta;
}
