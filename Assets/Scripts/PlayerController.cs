using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public ItemData baseWeapon;
    public Transform playerRoot;
    public Rigidbody2D rb;
    public Animator playerAnimator;
    public EnemyScanner enemyScanner;
    public float speed = 3f;
    public Vector2 inputVec;
    public bool useTouchControl = true; // 터치 컨트롤 활성화 여부

    private Vector3 currentRotation;
    private Vector2 touchStartPos;
    private bool isTouching = false;

    private Camera mainCamera; // 월드 좌표 변환을 위한 메인 카메라

    private void Awake()
    {
        InitializeComponents();
    }

    /// <summary>
    /// 필요한 컴포넌트들을 초기화하고 찾아서 할당
    /// </summary>
    private void InitializeComponents()
    {
        // null인 경우에만 컴포넌트를 찾아서 할당
        if (!playerRoot) playerRoot = transform.GetChild(0);
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!playerAnimator) playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        if (!enemyScanner) enemyScanner = GetComponent<EnemyScanner>();
        mainCamera = Camera.main; // 메인 카메라 캐시
    }

    /// <summary>
    /// 새 입력 시스템의 Move 입력 처리 (키보드/게임패드)
    /// </summary>
    private void OnMove(InputValue value)
    {
        // 게임이 진행중이 아니거나 터치 컨트롤을 사용중이면 무시
        if (!IsGameplayActive() || useTouchControl) return;
        inputVec = value.Get<Vector2>();
    }

    private void Update()
    {
        if (!IsGameplayActive()) return;
        HandleInput();
    }

    /// <summary>
    /// 터치/마우스 입력 처리 메인 로직
    /// </summary>
    private void HandleInput()
    {
        if (!useTouchControl) return;

        // 터치 입력이 있으면 터치 처리, 없으면 마우스 처리
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    /// <summary>
    /// 터치 입력 처리 - 모바일 디바이스용
    /// </summary>
    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);

        // UI 위의 터치는 무시
        if (IsPointerOverUI(touch.fingerId)) return;

        // 터치 좌표를 월드 좌표로 변환
        Vector2 touchWorldPos = mainCamera.ScreenToWorldPoint(touch.position);
        
        // 터치 단계별 처리
        switch (touch.phase)
        {
            case UnityEngine.TouchPhase.Began:           // 터치 시작
                StartTouch(touchWorldPos);
                break;
            case UnityEngine.TouchPhase.Moved:           // 터치 이동 중
            case UnityEngine.TouchPhase.Stationary:      // 터치 정지 상태
                UpdateTouchMovement(touchWorldPos);
                break;
            case UnityEngine.TouchPhase.Ended:           // 터치 종료
            case UnityEngine.TouchPhase.Canceled:        // 터치 취소
                EndTouch();
                break;
        }
    }

    /// <summary>
    /// 마우스 입력 처리 - PC용
    /// </summary>
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            // 마우스 클릭 시작
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            StartTouch(mouseWorldPos);
        }
        else if (Input.GetMouseButton(0))
        {
            // 마우스 드래그 중
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            UpdateTouchMovement(mouseWorldPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 마우스 클릭 종료
            EndTouch();
        }
    }

    /// <summary>
    /// 터치/드래그 시작 시 처리
    /// </summary>
    private void StartTouch(Vector2 worldPosition)
    {
        touchStartPos = worldPosition;
        isTouching = true;
        CalculateInputVector(worldPosition);
    }

    /// <summary>
    /// 터치/드래그 이동 중 처리
    /// </summary>
    private void UpdateTouchMovement(Vector2 worldPosition)
    {
        CalculateInputVector(worldPosition);
    }

    /// <summary>
    /// 플레이어 위치 기준으로 이동 방향 계산
    /// </summary>
    private void CalculateInputVector(Vector2 worldPosition)
    {
        // 플레이어 위치에서 터치 위치까지의 방향 벡터 계산
        Vector2 direction = worldPosition - (Vector2)transform.position;
        inputVec = direction.normalized; // 방향만 사용 (거리는 무시)
    }

    /// <summary>
    /// 터치/드래그 종료 시 처리
    /// </summary>
    private void EndTouch()
    {
        inputVec = Vector2.zero;
        isTouching = false;
    }

    /// <summary>
    /// UI 요소 위에 포인터가 있는지 확인
    /// </summary>
    private bool IsPointerOverUI(int? touchId = null)
    {
        if (EventSystem.current == null) return false;
        return touchId.HasValue ? 
            EventSystem.current.IsPointerOverGameObject(touchId.Value) : 
            EventSystem.current.IsPointerOverGameObject();
    }

    private void FixedUpdate()
    {
        if (!IsGameplayActive()) return;
        UpdateMovement();
    }

    /// <summary>
    /// 실제 물리 기반 이동 처리
    /// </summary>
    private void UpdateMovement()
    {
        // 현재 위치에서 입력 방향으로 이동
        Vector2 nextPosition = rb.position + (inputVec * speed * Time.fixedDeltaTime);
        rb.MovePosition(nextPosition);
    }

    private void LateUpdate()
    {
        if (!IsGameplayActive()) return;
        UpdateAnimation();
        UpdateRotation();
    }

    /// <summary>
    /// 이동 애니메이션 업데이트
    /// </summary>
    private void UpdateAnimation()
    {
        playerAnimator.SetFloat("Speed", inputVec.magnitude);
    }

    /// <summary>
    /// 캐릭터 좌우 방향 회전 처리
    /// </summary>
    private void UpdateRotation()
    {
        if (inputVec.x == 0) return;
        
        currentRotation = playerRoot.localEulerAngles;
        currentRotation.y = inputVec.x > 0 ? 180f : 0f; // 이동 방향에 따라 좌우 반전
        playerRoot.localEulerAngles = currentRotation;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!IsGameplayActive()) return;
        HandleDamage();
    }

    /// <summary>
    /// 데미지 처리 및 게임 오버 체크
    /// </summary>
    private void HandleDamage()
    {
        // 초당 10의 데미지
        GameManager.instance.health -= Time.deltaTime * 10f;

        // 체력이 0 이하면 게임 오버 처리
        if (GameManager.instance.health <= 0)
        {
            DisableChildObjects();
            TriggerDeathAnimation();
            GameManager.instance.GameOver();
        }
    }

    /// <summary>
    /// 사망 시 자식 오브젝트 비활성화
    /// </summary>
    private void DisableChildObjects()
    {
        // 첫 번째와 두 번째 자식을 제외한 모든 자식 오브젝트 비활성화
        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 사망 애니메이션 재생
    /// </summary>
    private void TriggerDeathAnimation()
    {
        playerAnimator.SetTrigger("Dead");
    }

    /// <summary>
    /// 게임이 현재 진행 중인지 확인
    /// </summary>
    private bool IsGameplayActive()
    {
        return GameManager.instance.isLive;
    }
}
