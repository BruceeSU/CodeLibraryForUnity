using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LitonLib.CustomComponent.Cameras
{
    public class UltimateCameraBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 摄像机类型
        /// </summary>
        public enum CameraType
        {
            SpaceCraft,
            PlaneFirstPerson,
            PlaneThirdPerson,
            SphereFirstPerson,
            SphereThirdPerson,
            CenterMode,
            TwoDMode,
        }

        private class Consts
        {
            public const string MouseX = "Mouse X";
            public const string MouseY = "Mouse Y";
            public const string MouseWheel = "Mouse ScrollWheel";
            public const string Horizontal = "Horizontal";
            public const string Vertical = "Vertical";
        }

        public CameraType cameraType;
        public SpaceCraftModeData spaceCraftModeData;
        public CenterModeData centerModeData;
        public TwoDModeData twoDModeData;
        public SphereThirdPersonModeData sphere3rdMode;
        private Camera _cam;
        private Transform _camTrans;
        public Camera Cam
        {
            get
            {
                if (_cam == null) _cam = GetComponent<Camera>();
                return _cam;
            }
        }
        void Start()
        {
            InitSetting();
        }


        void Update()
        {
            switch (cameraType)
            {
                case CameraType.SpaceCraft:
                    SpaceCraftMode();
                    break;
                case CameraType.PlaneFirstPerson:
                    PlaneFirstPersonMode();
                    break;
                case CameraType.PlaneThirdPerson:
                    PlaneThirdPersonMode();
                    break;
                case CameraType.SphereFirstPerson:
                    SphereFirstPersonMode();
                    break;
                case CameraType.SphereThirdPerson:
                    SphereThirdPersonMode();
                    break;
                case CameraType.CenterMode:
                    CenterMode();
                    break;
                case CameraType.TwoDMode:
                    TwoDMode();
                    break;
                default:
                    break;
            }
        }

        void InitSetting()
        {
            // twoDModeData.rect = new Vector4(-5f, 5f, 5f, -5f);
            _camTrans = transform;
            sphere3rdMode.Initialize();
        }

        private void SpaceCraftMode()
        {
            transform.position += (Input.GetAxis(Consts.Vertical) * transform.forward + Input.GetAxis(Consts.Horizontal) * transform.right) * Time.deltaTime * spaceCraftModeData.moveSpeed;
            transform.RotateAround(transform.position, transform.right, -Input.GetAxis(Consts.MouseY) * Time.deltaTime * spaceCraftModeData.rotateSpeed);
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis(Consts.MouseX) * Time.deltaTime * spaceCraftModeData.rotateSpeed);
        }
        private void PlaneFirstPersonMode()
        { }

        private void PlaneThirdPersonMode()
        { }

        private void SphereFirstPersonMode()
        { }

        private void SphereThirdPersonMode()
        {


            float deltaTime = sphere3rdMode.ignoreTimeScale ? Time.fixedDeltaTime : Time.deltaTime;
            //zoom
            sphere3rdMode.zoomDistance += Input.GetAxis(Consts.MouseWheel) * sphere3rdMode.zoomSpeed * deltaTime;
            sphere3rdMode.zoomDistance = Mathf.Clamp(sphere3rdMode.zoomDistance, sphere3rdMode.zoomMinDistance, sphere3rdMode.zoomMaxDistance);
            Vector3 right = _camTrans.right;
            //rotate
            if (Input.GetMouseButton(1))
            {
                float vAngle = -Input.GetAxis(Consts.MouseY) * deltaTime * sphere3rdMode.rotateSpeed;
                float hAngle = Input.GetAxis(Consts.MouseX) * deltaTime * sphere3rdMode.rotateSpeed;
                RotateSphereCamera(vAngle, hAngle, right, sphere3rdMode.center2TargetPos, false);
            }

            //move
            if (Input.GetMouseButton(2))
            {
                float vAngle = Input.GetAxis(Consts.MouseY) * deltaTime * sphere3rdMode.moveSpeed;
                float hAngle = Input.GetAxis(Consts.MouseX) * deltaTime * sphere3rdMode.moveSpeed;
                sphere3rdMode.center2TargetPos = Quaternion.AngleAxis(vAngle, right) * sphere3rdMode.center2TargetPos;
                Vector3 fwdAxis = _camTrans.up;// Vector3.Cross(sphere3rdMode.center2TargetPos.normalized, _camTrans.right);
                sphere3rdMode.center2TargetPos = Quaternion.AngleAxis(hAngle, fwdAxis) * sphere3rdMode.center2TargetPos;

                RotateSphereCamera(vAngle, hAngle, right, fwdAxis, true);
            }
            _camTrans.localRotation = sphere3rdMode.GetRotation();
            _camTrans.position = sphere3rdMode.GetPosition();

        }

        private void RotateSphereCamera(float vAngle, float hAngle, Vector3 vAxis, Vector3 hAxis, bool move)
        {
            sphere3rdMode.targetPos2Camera = Quaternion.AngleAxis(vAngle, vAxis) * sphere3rdMode.targetPos2Camera;
            float angle = Vector3.Angle(sphere3rdMode.center2TargetPos, sphere3rdMode.targetPos2Camera);
            angle = Mathf.Clamp(angle, sphere3rdMode.minAngle, sphere3rdMode.maxAngle);
            sphere3rdMode.targetPos2Camera = Quaternion.AngleAxis(angle, vAxis) * sphere3rdMode.center2TargetPos.normalized;
            if (!move) sphere3rdMode.targetPos2Camera = Quaternion.AngleAxis(hAngle, hAxis) * sphere3rdMode.targetPos2Camera;
            sphere3rdMode.targetPos2Camera.Normalize();

        }

        private void CenterMode()
        {
            float deltaTime = centerModeData.ignoreTimeScale ? 0.02f : Time.deltaTime;
            if (Input.GetMouseButton(1))
            {
                transform.RotateAround(centerModeData.center, Vector3.up, Input.GetAxis(Consts.MouseX) * deltaTime * centerModeData.rotateSpeed);
                transform.RotateAround(centerModeData.center, -transform.right, Input.GetAxis(Consts.MouseY) * deltaTime * centerModeData.rotateSpeed);
            }
            if (Input.GetAxis(Consts.MouseWheel) != 0f)
            {
                centerModeData.distanceToCenter += Input.GetAxis(Consts.MouseWheel) * centerModeData.roomSpeed * deltaTime;
                centerModeData.distanceToCenter = Mathf.Clamp(centerModeData.distanceToCenter, centerModeData.minDistance, centerModeData.maxDistance);
            }
            transform.LookAt(centerModeData.center);
            transform.position = centerModeData.center - transform.forward * centerModeData.distanceToCenter;
            /*
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetColor("_ColorBlock",new Color());
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(mpb);
            */
        }

        private void TwoDMode()
        {
            if (Input.GetAxis(Consts.MouseWheel) != 0f)
            {
                float size = Cam.orthographicSize;
                float roomSpeed = twoDModeData.roomSpeed * Cam.orthographicSize;
                size += Input.GetAxis(Consts.MouseWheel) * twoDModeData.roomSpeed * Time.deltaTime;
                size = Mathf.Clamp(size, 1f, 5f);
                Cam.orthographicSize = size;
                ClampCameraPos(transform.position);
            }

            if (Input.GetMouseButton(1))
            {
                twoDModeData.pos = transform.position - new Vector3(Input.GetAxis(Consts.MouseX), 0f, Input.GetAxis(Consts.MouseY)) * twoDModeData.moveSpeed * Time.deltaTime;
                ClampCameraPos(twoDModeData.pos);
            }
        }

        private void ClampCameraPos(Vector3 pos)
        {
            float up = twoDModeData.rect.z - Cam.orthographicSize;
            float bottom = twoDModeData.rect.w + Cam.orthographicSize;
            float left = twoDModeData.rect.x + Cam.orthographicSize / Cam.aspect;
            float right = twoDModeData.rect.y - Cam.orthographicSize / Cam.aspect;

            pos.x = Mathf.Clamp(pos.x, left, right);
            pos.z = Mathf.Clamp(pos.z, bottom, up);
            transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 from = sphere3rdMode.sphereCenter + sphere3rdMode.center2TargetPos.normalized * sphere3rdMode.sphereRaidus;
            Gizmos.DrawLine(from, from + sphere3rdMode.targetPos2Camera * 20f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(sphere3rdMode.sphereCenter, sphere3rdMode.sphereCenter + sphere3rdMode.center2TargetPos * 50);
        }
    }

    /// <summary>
    /// 飞行模式的运动数据
    /// </summary>
    [System.Serializable]
    public struct SpaceCraftModeData
    {
        public float moveSpeed;
        public float rotateSpeed;

    }

    /// <summary>
    /// 平地模式第三人称相机的运动数据
    /// </summary>
    public struct PlaneThirdPersonModeData
    {
        /// <summary>
        /// 相机跟随阻尼
        /// </summary>
        public float slerpValue;

    }
    public struct PlaneFirstPersonModeData
    { }

    public struct SphereFistPersonModeData
    { }

    [System.Serializable]
    public struct SphereThirdPersonModeData
    {
        public Transform targetObj;
        public Vector3 sphereCenter;
        public Vector3 targetPos;
        public Vector3 center2TargetPos;
        public Vector3 targetPos2Camera;
        public float sphereRaidus;
        public float zoomSpeed;
        public float zoomDistance;
        public float zoomMinDistance;
        public float zoomMaxDistance;
        public float moveSpeed;
        public float rotateSpeed;
        public float minAngle;
        public float maxAngle;

        public bool ignoreTimeScale;

        public void Initialize()
        {
            sphereCenter = GameObject.Find("Sphere").transform.position;
            if (center2TargetPos == Vector3.zero) center2TargetPos = Vector3.up;
            targetPos = sphereCenter + center2TargetPos.normalized * sphereRaidus;
            if (targetPos2Camera == Vector3.zero) targetPos2Camera = Vector3.one.normalized;
        }

        public Vector3 GetPosition()
        {
            return sphereCenter + center2TargetPos.normalized * sphereRaidus + targetPos2Camera.normalized * zoomDistance;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.LookRotation(targetPos2Camera.normalized, center2TargetPos.normalized);
        }

    }

    [System.Serializable]
    public struct CenterModeData
    {
        public Vector3 center;
        public float distanceToCenter;
        public float maxDistance;
        public float minDistance;
        public float rotateSpeed;
        public float roomSpeed;

        public bool ignoreTimeScale;
    }


    /// <summary>
    /// 平面模式的属性
    /// </summary>
    [System.Serializable]
    public struct TwoDModeData
    {
        public float moveSpeed;
        public float roomSpeed;
        public float minSize;
        public float maxSize;
        public Vector4 rect;
        public Vector3 pos;
    }
}