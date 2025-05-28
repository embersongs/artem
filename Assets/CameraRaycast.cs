using UnityEngine;
using UnityEngine.UIElements;

public class OffsetRaycast : MonoBehaviour
{
    [Header("Настройки луча")]
    public float rayLength = 100f;
    public Vector3 rayOffset = new Vector3(0.3f, -0.2f, 0.2f); // Смещение от центра камеры

    [Header("Визуализация")]
    public bool showDebugRay = true;
    public Color rayColor = Color.red;

    private LineRenderer lineRenderer;
    private Camera playerCamera;
    private GameObject currentMarker;
    private float lastMarkerTime; // Время последнего создания маркера

    [Header("Настройки маркера")]
    public GameObject hitMarkerPrefab;
    public float markerLifetime = 2f;
    public float markerCooldown = 0.1f; // Интервал между созданиями маркеров

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        // Настройка LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Рассчитываем точку испускания луча с учетом смещения
        Vector3 rayOrigin = playerCamera.transform.position +
                           playerCamera.transform.right * rayOffset.x +
                           playerCamera.transform.up * rayOffset.y +
                           playerCamera.transform.forward * rayOffset.z;

        // Направление луча (вперед от камеры, но можно изменить)
        Vector3 rayDirection = playerCamera.transform.forward;

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        bool rayHit = Physics.Raycast(ray, out hit, rayLength);

        if (rayHit)
        {
            string objectTag = hit.collider.tag;

            // Создание маркера с задержкой
            if (rayHit && Time.time - lastMarkerTime >= markerCooldown)
            {
                CreateMarker(hit.point);
                lastMarkerTime = Time.time;
            }

            // Удаление маркера при промахе
            if (!rayHit && currentMarker != null)
            {
                Destroy(currentMarker);
                currentMarker = null;
            }


            Debug.Log($"Попали в объект: {objectTag} ({hit.collider.name})");

            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, rayOrigin);
            lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);
        }

        // Дополнительная визуализация в редакторе
        if (showDebugRay)
        {
            Debug.DrawRay(rayOrigin, rayDirection * (hit.collider ? hit.distance : rayLength), rayColor);
        }
    }


    void CreateMarker(Vector3 position)
    {
  

        // Создаём новый маркер
        if (hitMarkerPrefab != null)
        {
            currentMarker = Instantiate(hitMarkerPrefab, position, Quaternion.identity);
        }
        else
        {
            currentMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            currentMarker.transform.position = position;
            currentMarker.transform.localScale = Vector3.one * 0.1f;
            Destroy(currentMarker.GetComponent<Collider>());
        }

        Destroy(currentMarker, markerLifetime);
    }
}
