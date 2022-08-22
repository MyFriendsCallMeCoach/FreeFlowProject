using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class LedgeClimb : MonoBehaviour
{
    public static LedgeClimb instance;

    AdvancedWalkerController controller;
    private Animator CharAnimator;
    private Rigidbody Rigidbody;
    private CapsuleCollider CC_Capsule;

    private Vector3 EndPos;
    private RaycastHit DownRayHit;
    private RaycastHit UpRayHit;

    public bool Climbing;
    public Vector3 _endPosition;

    [Header("Climb Settings")]
    [SerializeField] private float WallAngleMax;
    [SerializeField] private float GroundAngleMax;
    [SerializeField] private LayerMask ClimbableLayer;

    [Header("height")]
    [SerializeField] private float OverPassHeight;
    [SerializeField] private float StepHeight;
    [SerializeField] private float VaultHeight;
    [SerializeField] private float HangHeight;
    [SerializeField] private float ClimbUpHeight;

    [Header("Offset")]
    [SerializeField] private Vector3 ClimbOriginDown;
    [SerializeField] private Vector3 EndOffset;

    [Header("Animation Settings")]
    public CrossFadeSettings StandToFreeHangSettings;
    public CrossFadeSettings ClimbUpSettings;
    public CrossFadeSettings VaultSettings;
    public CrossFadeSettings StepUpSettings;
    public CrossFadeSettings DropSettings;
    public CrossFadeSettings DropToAir;


    private void Awake()
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AdvancedWalkerController>();
        CharAnimator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        CC_Capsule = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!Climbing)
        {
            if(CanClimb(out DownRayHit, out UpRayHit, out EndPos))
            {
                if (AdvancedWalkerController.instance.currentControllerState == AdvancedWalkerController.ControllerState.Jumping || AdvancedWalkerController.instance.currentControllerState == AdvancedWalkerController.ControllerState.Falling)
                {
                    InitiateClimb();

                }
            }
        }
    }


   

    public bool CanClimb(out RaycastHit DownRayCastHit, out RaycastHit ForwardRayCastHit, out Vector3 EndPosition)
    {
        EndPosition = Vector3.zero;

        DownRayCastHit = new RaycastHit();

        ForwardRayCastHit = new RaycastHit();

        bool _DownHit;
        bool _ForwardHit;
        bool _OverpassHit;

        float _ClimbingHeight;
        float _GroundAngle;
        float _WallAngle;

        RaycastHit _DownRaycastHit;
        RaycastHit _ForwardRaycastHit;
        RaycastHit _OverpassRaycastHit;

        Vector3 _EndPosition;
        Vector3 _ForwardDirectionXZ;
        Vector3 _ForwardNormalXZ;

        Vector3 _DownDirection = Vector3.down;
        Vector3 _DownOrigin = transform.TransformPoint(ClimbOriginDown);

        _DownHit = Physics.Raycast(_DownOrigin, _DownDirection, out _DownRaycastHit, ClimbOriginDown.y - StepHeight, ClimbableLayer);
        if(_DownHit)
        {
            //We get the forward + overpass cast
            float _ForwardDistance = ClimbOriginDown.z;
            Vector3 _ForwardOrigin = new Vector3(transform.position.x, _DownRaycastHit.point.y - 0.1f, transform.position.z);
            Vector3 _OverpassOrigin = new Vector3(transform.position.x, OverPassHeight, transform.position.z);

            _ForwardDirectionXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            _ForwardHit = Physics.Raycast(_ForwardOrigin, _ForwardDirectionXZ, out _ForwardRaycastHit, ClimbableLayer);
            _OverpassHit = Physics.Raycast(_OverpassOrigin, _ForwardDirectionXZ, out _OverpassRaycastHit, ClimbableLayer);
            _ClimbingHeight = _DownRaycastHit.point.y - transform.position.y;

            if(_ForwardHit)
                if(_OverpassHit || _ClimbingHeight < OverPassHeight)
                {
                    _ForwardNormalXZ = Vector3.ProjectOnPlane(_ForwardRaycastHit.normal, Vector3.up);
                    _GroundAngle = Vector3.Angle(_DownRaycastHit.normal, Vector3.up);
                    _WallAngle = Vector3.Angle(-_ForwardNormalXZ, _ForwardDirectionXZ);

                    if(_WallAngle <= WallAngleMax)
                    {
                        if(_GroundAngle <= GroundAngleMax)
                        {
                            //We get the end offset
                            Vector3 _VectSurface = Vector3.ProjectOnPlane(_ForwardDirectionXZ, _DownRaycastHit.normal);
                            _EndPosition = _DownRaycastHit.point + Quaternion.LookRotation(_VectSurface, Vector3.up) * EndOffset;

                            //De-Penetration
                            Collider _ColliderB = _DownRaycastHit.collider;
                            bool _PenOverlap = Physics.ComputePenetration(
                                colliderA: CC_Capsule,
                                positionA: _EndPosition,
                                rotationA: transform.rotation,
                                colliderB: _ColliderB,
                                positionB: _ColliderB.transform.position,
                                rotationB: _ColliderB.transform.rotation,
                                direction: out Vector3 _PenetrationDirection,
                                distance: out float _PenetrationDistance);

                            if(_PenOverlap)
                                _EndPosition += _PenetrationDirection * _PenetrationDistance;


                            //up sweep

                            float _Inflate = -0.05f;
                            float _UpSweepDistance = _DownRaycastHit.point.y - transform.position.y;

                            Vector3 _UpSweepDirection = transform.up;
                            Vector3 _UpSweepOrigin = transform.position;

                            bool _UpSweepHit = CharacterSweep(
                                position: _UpSweepOrigin,
                                rotation: transform.rotation,
                                direction: _UpSweepDirection,
                                distance: _UpSweepDistance,
                                layerMask: ClimbableLayer,
                                inflate: _Inflate);

                            //Forward Sweep
                            Vector3 _ForwardSweepOrigin = transform.position + _UpSweepDirection * _UpSweepDistance;
                            Vector3 _ForwardSweepVector = _EndPosition - _ForwardSweepOrigin;

                            bool _ForwardSweepHit = CharacterSweep(
                                    position: _UpSweepOrigin,
                                    rotation: transform.rotation,
                                    direction: _ForwardSweepVector.normalized,
                                    distance: _ForwardSweepVector.magnitude,
                                    layerMask: ClimbableLayer,
                                    inflate: _Inflate);


                            if(!_UpSweepHit && !_ForwardSweepHit)
                            {
                                EndPosition = _EndPosition;
                                DownRayCastHit = _DownRaycastHit;
                                ForwardRayCastHit = _ForwardRaycastHit;

                                return true;
                            }



                        }
                    }
                }

        }

        return false;
    }

    public bool CharacterSweep(Vector3 position, Quaternion rotation, Vector3 direction, float distance, LayerMask layerMask, float inflate)
    {
        float _HeightScale = Mathf.Abs(transform.lossyScale.y);
        float _RadiusScale = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.z));

        float _Radius = CC_Capsule.radius * _RadiusScale;
        float _TotalHeight = Mathf.Max(CC_Capsule.height * _HeightScale, _Radius * 2);

        Vector3 _CapsuleUp = rotation * Vector3.up;
        Vector3 _Center = position + rotation * CC_Capsule.center;
        Vector3 _Top = _Center + _CapsuleUp * (_TotalHeight / 2 - _Radius);
        Vector3 _Bottom = _Center - _CapsuleUp * (_TotalHeight / 2 - _Radius);


        bool _SweepHit = Physics.CapsuleCast(
            point1: _Bottom,
            point2: _Top,
            radius: _Radius,
            direction: direction,
            maxDistance: distance,
            layerMask: layerMask);
        
        return _SweepHit;
    }

    public void Climb()
    {
        if (Climbing)
        {
            if(CanClimb(out DownRayHit, out UpRayHit, out EndPos))
            {
                if (AdvancedWalkerController.instance.currentControllerState == AdvancedWalkerController.ControllerState.Jumping || AdvancedWalkerController.instance.currentControllerState == AdvancedWalkerController.ControllerState.Falling)
                {
                    InitiateClimb();

                }
            }

        }
    }


    public void InitiateClimb()
    {
        Climbing = true;
        CharAnimator.SetFloat("Forward", 0);
        CC_Capsule.enabled = false;
        Rigidbody.isKinematic = true;


        float _ClimbHeight = DownRayHit.point.y - transform.position.y;
        
        if(_ClimbHeight > HangHeight)
        {
            CharAnimator.CrossFadeInFixedTime(StandToFreeHangSettings);
        }
        else if (_ClimbHeight > ClimbUpHeight)
        {
            CharAnimator.CrossFadeInFixedTime(ClimbUpSettings);
        }
        else if (_ClimbHeight > VaultHeight)
        {
            CharAnimator.CrossFadeInFixedTime(VaultSettings);
        }
        else if (_ClimbHeight > StepHeight)
        {
            CharAnimator.CrossFadeInFixedTime(StepUpSettings);
        }
        else
        {
            Climbing = false;
        }

    }
}
