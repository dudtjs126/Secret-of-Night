using UnityEngine;

//카메라 위치 오프셋 벡터, 피봇 오프셋 벡터
//위치 오프셋은 충돌처리용, 피봇은 시선이동에
//FOV : 시야각 변경 기능

[RequireComponent(typeof(Camera))]
public class CameraTPP : MonoBehaviour
{
    public Transform player;
    public Vector3 pivotOffset = new Vector3(0.0f, 1.05f, -1.04f);
    public Vector3 camOffset = new Vector3(0.4f, 0.5f, -2.0f);

    public float smooth = 10f; // 카메라 반응속도
    public float horizontalAimingSpeed = 6.0f; // 수평 회전속도
    public float verticalAimingSpeed = 6.0f; // 수직 회전속도
    public float maxVerticalAngle = 30.0f; // 최대 수직각도
    public float minVerticalAngle = -60.0f;// 최소 수직각도

    //smoothDamp
    float angleHVelocity = 0.0f;
    Vector3 pivotOffsetVelocity = Vector3.zero;
    Vector3 camOffsetVelocity = Vector3.zero;
    float smoothTime = 0.3f;

    private float angleH = 0.0f; // 마우스 이동에 따른 카메라 수평이동 수치
    private float angleV = 0.0f; // 마우스 이동에 따른 카메라 수직이동 수치
    private Transform cameraTransform; // 카메라 Transform 캐싱용
    private Camera myCamera; // FOV 수정용

    private Vector3 relCameraPos; // 플레이어부터 카메라 까지의 벡터
    private float relCameraPosMag; // 플레이어부터 카메라 사이의 거리
    private Vector3 smoothPivotOffset; // 카메라 피봇용 보간용 벡터
    private Vector3 smoothCamOffset; // 카메라 위치용 보간용 벡터
    private Vector3 targetPivotOffset; // 카메라 피봇용 보간용 벡터
    private Vector3 targetCamOffset; // 카메라 위치용 보간용 벡터

    private float defaultFOV; // 기본 시야값
    private float targetFOV; // 타켓 시야값
    private float targetMaxVerticalAngle;// 카메라 수직 최대각도
    private float targetHorizontalAngle;
    public float GetH => angleH;

    private void Awake()
    {
        //캐싱
        cameraTransform = transform;
        myCamera = cameraTransform.GetComponent<Camera>();

        //카메라 기본 포지션 세팅
        cameraTransform.position = player.position + pivotOffset + camOffset;
        cameraTransform.rotation = Quaternion.identity;

        //카메라와 플레이어 간의 상태 벡터
        relCameraPos = cameraTransform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f; // 플레이어가 raycast에 들어가지 않게 하기위해 0.5f 제거 -> 추후 layerMask로 변환도 고려

        //기본 세팅
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defaultFOV = myCamera.fieldOfView;
        angleH = player.eulerAngles.y;

        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();
    }

    public void ResetTargetOffsets()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }
    public void ResetFOV()
    {
        targetFOV = defaultFOV;
    }
    public void ResetMaxVerticalAngle()
    {
        targetMaxVerticalAngle = maxVerticalAngle;
    }
    public void SetTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;
    }
    public void SetFOV(float customFOV)
    {
        targetFOV = customFOV;
    }
    public void SetAngleH(float degree)
    {
        targetHorizontalAngle = degree;
    }
    public void ResetAngleH()
    {
        targetHorizontalAngle = 0;
    }
    private void Update()
    {
        ResetTargetOffsets();
        //마우스 이동값
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f) * verticalAimingSpeed;

        //수직 이동 제한
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

        //실험용
        //angleV = -40f;
        //angleH = Mathf.LerpAngle(angleH, targetHorizontalAngle, Time.deltaTime);
        angleH = Mathf.SmoothDampAngle(angleH, targetHorizontalAngle, ref angleHVelocity, smooth * 0.1f);
        angleH = Mathf.Clamp(angleH, -45f, 45f);

        //카메라 회전
        Quaternion camYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);
        cameraTransform.rotation = aimRotation;

        //setFOV
        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, targetFOV, Time.deltaTime);

        //카메라 충돌처리
        Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset; // 조준할때와 평소에 다르다

        ViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset,
             0.7f, ref noCollisionOffset);



        //Reposition Camera
        smoothPivotOffset = Vector3.SmoothDamp(smoothPivotOffset, targetPivotOffset, ref pivotOffsetVelocity, smoothTime);
        smoothCamOffset = Vector3.SmoothDamp(smoothCamOffset, noCollisionOffset, ref camOffsetVelocity, smoothTime);

        cameraTransform.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
    }

    //player의 pivot position이 발바닭임을 가정한다 deltaPlayerHeight만큼의 위에서 체크    
    bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, ref Vector3 offSet)
    {
        Vector3 origin = player.position;
        Debug.DrawRay(origin, checkPos - origin, Color.red);
        Vector3 direction = checkPos - origin;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, direction.magnitude))
        {
            if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                Debug.DrawRay(hit.point, Vector3.up);
                float dirMag = (checkPos - hit.point).magnitude + 0.1f;
                offSet.z += dirMag;
                //Debug.Log(dirMag);
                //Debug.Log(offSet.z);
                offSet.z = Mathf.Clamp(offSet.z, -1000f, 0.3f);
                return false;
            }
        }
        return true;
    }
    //CapsuleCollider을 사용해 플레이어의 목 부분 위치를 가져와 이 지점을 기점으로 카메라와 플레이어 사이의 물체 감지

    public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    }
}
