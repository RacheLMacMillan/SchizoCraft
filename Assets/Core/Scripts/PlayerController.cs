using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 7f;
    
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    
    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();
        
        // Заблокировать курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        
        // Проверяем наличие камеры
        if (playerCamera == null)
        {
            // Ищем камеру среди дочерних объектов
            playerCamera = GetComponentInChildren<Camera>()?.transform;
            if (playerCamera == null)
            {
                Debug.LogError("Camera not assigned! Please assign a camera transform.");
            }
        }
    }

    void Update()
    {
        // Обработка ввода мыши для вращения камеры
        HandleMouseLook();
        
        // Обработка прыжка
        HandleJump();
    }

    void FixedUpdate()
    {
        // Обработка движения WASD
        HandleMovement();
        
        // Проверка нахождения на земле
        CheckGrounded();
    }

    void HandleMouseLook()
    {
        // Получаем ввод мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вращение игрока по горизонтали (влево/вправо)
        transform.Rotate(Vector3.up * mouseX);

        // Вращение камеры по вертикали (вверх/вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Ограничиваем угол обзора
        
        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    void HandleMovement()
    {
        // Получаем ввод с клавиатуры (WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Вычисляем направление движения относительно поворота игрока
        Vector3 movement = (transform.forward * vertical + transform.right * horizontal).normalized;
        Vector3 targetVelocity = movement * moveSpeed;

        // Сохраняем текущую Y-скорость (для гравитации)
        targetVelocity.y = rb.linearVelocity.y;

        // Применяем движение
        rb.linearVelocity = targetVelocity;
    }

    void HandleJump()
    {
        // Прыжок при нажатии Space и если персонаж на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void CheckGrounded()
    {
        // Проверяем, касается ли персонаж земли
        RaycastHit hit;
        float rayLength = 1.1f; // Немного больше половины высоты персонажа
        Vector3 rayStart = transform.position;

        // Бросаем луч вниз
        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayLength))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Визуализация луча для отладки
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);
    }

    // Разблокировать курсор при отключении скрипта
    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}