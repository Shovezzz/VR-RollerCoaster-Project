using UnityEngine;
using Futurift;
using Futurift.DataSenders;
using Futurift.Options;

public class FuturiftController_Handler : MonoBehaviour
{
    [Header("Настройки подключения")]
    [SerializeField] private string ipAddress = "127.0.0.1";
    [SerializeField] private int port = 6065;

    [Header("Настройки симуляции")]
    [Tooltip("Сила реакции на ускорение/торможение")]
    [Range(0f, 20f)]
    [SerializeField] private float accelerationFactor = 5f;

    [Tooltip("Сила с которой гравитация вжимает на подъемах")]
    [Range(0f, 5f)]
    [SerializeField] private float gravityFactor = 0.5f;

    [Tooltip("Общий множитель для крена")]
    [Range(0f, 2f)]
    [SerializeField] private float rollFactor = 1.0f;

    [Tooltip("Плавность реакции на ускорение. Чем выше тем более сглаженно")]
    [Range(1f, 20f)]
    [SerializeField] private float accelerationSmoothing = 10f;

    [Tooltip("Общая плавность движений капсулы. Чем выше тем быстрее реакция")]
    [Range(1f, 20f)]
    [SerializeField] private float overallDamping = 5f;

    private FutuRiftController _futuriftController;

    // Переменные для расчетов
    private Vector3 _lastPosition;
    private float _lastForwardVelocity = 0f;
    private float _smoothedAcceleration = 0f;

    void Awake()
    {
        var udpOptions = new UdpOptions { ip = ipAddress, port = port };
        var futuRiftOptions = new FutuRiftOptions { interval = 20 }; 

        _futuriftController = new FutuRiftController(
            dataSender: new UdpPortSender(udpOptions),
            futuRiftOptions: futuRiftOptions
        );
    }

    void OnEnable()
    {
        _futuriftController?.Start();
        _lastPosition = transform.position;
        _lastForwardVelocity = 0f;
        _smoothedAcceleration = 0f;
    }

    void OnDisable()
    {
        if (_futuriftController != null)
        {
            _futuriftController.Pitch = 0;
            _futuriftController.Roll = 0;
            _futuriftController.Stop();
        }
    }

    // ИСПОЛЬЗУЕМ LATEUPDATE, ЧТОБЫ ЧИТАТЬ ПОЗИЦИЮ ПОСЛЕ SPLINE FOLLOWER
    void LateUpdate()
    {
        if (_futuriftController == null || Time.deltaTime == 0) return;

        // Расчет сырого ускорения 
        Vector3 currentVelocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;

        float currentForwardVelocity = transform.InverseTransformDirection(currentVelocity).z;
        float rawAcceleration = (currentForwardVelocity - _lastForwardVelocity) / Time.deltaTime;
        _lastForwardVelocity = currentForwardVelocity;

        // Сглаживангие ускорения
        _smoothedAcceleration = Mathf.Lerp(
            _smoothedAcceleration,
            rawAcceleration,
            Time.deltaTime * accelerationSmoothing
        );

        // Расчет гравитации и крена
        float trackInclineAngle = Vector3.SignedAngle(Vector3.up, transform.up, transform.right);
        float trackRollAngle = Vector3.SignedAngle(
            Vector3.ProjectOnPlane(transform.up, transform.forward),
            Vector3.up,
            transform.forward
        );
        if (Mathf.Abs(currentForwardVelocity) < 3.0f || Mathf.Abs(trackRollAngle) < 0.5f) // Порог скорости и в градусах
        {
            trackRollAngle = 0f;
        }

        // Финальный расчет целевых углов
        float targetPitch = (_smoothedAcceleration * accelerationFactor) + (trackInclineAngle * gravityFactor);
        float targetRoll = trackRollAngle * rollFactor;

        // Плавное применение к капсуле 
        _futuriftController.Pitch = Mathf.Lerp(_futuriftController.Pitch, targetPitch, Time.deltaTime * overallDamping);
        _futuriftController.Roll = Mathf.Lerp(_futuriftController.Roll, targetRoll, Time.deltaTime * overallDamping);
    }
}