using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FpsCameraEffects : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;

    [Header("Headbob")]
    public bool enableHeadbob = true;
    public float headbobFrequency = 1.8f;     
    public float headbobAmplitude = 0.05f;    
    public float headbobRunMultiplier = 1.5f; 
    public float headbobSmoothing = 10f;

    [Header("Jump / Land Bob")]
    public bool enableJumpLandBob = true;
    public float jumpBobAmount = 0.05f;
    public float landBobAmount = 0.1f;
    public float jumpLandBobReturnSpeed = 8f;

    [Header("Camera Shake")]
    public bool enableShake = true;
    public float defaultShakeMagnitude = 0.1f;
    public float defaultShakeDuration = 0.1f;
    public float shakeFrequency = 25f;

    [Header("FOV Kick")]
    public bool enableFovKick = true;
    public float sprintFovIncrease = 8f;      
    public float fovLerpSpeed = 10f;          
    public float minSpeedForFovKick = 0.2f;   

    private Camera _cam;
    private Vector3 _initialLocalPos;

    private float _headbobTimer;
    private Vector3 _currentHeadbobOffset;

    private float _jumpLandOffsetY;
    private bool _wasGroundedLastFrame;

    private float _shakeTimer;
    private float _shakeDuration;
    private float _shakeMagnitude;
    private Vector3 _shakeOffset;
    private float _shakeSeed;

    private float _baseFov;
    private float _targetFov;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _initialLocalPos = player.cameraLocalOffset;
        _shakeSeed = Random.value * 1000f;

        _baseFov = _cam.fieldOfView;   
        _targetFov = _baseFov;
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateHeadbob();
        UpdateJumpLandBob();
        UpdateShake();
        UpdateFovKick();

        Vector3 totalOffset = _currentHeadbobOffset
                            + new Vector3(0f, _jumpLandOffsetY, 0f)
                            + _shakeOffset;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _initialLocalPos + totalOffset,
            Time.deltaTime * headbobSmoothing
        );

        _wasGroundedLastFrame = player.IsGrounded;
    }
    private void UpdateHeadbob()
    {
        if (!enableHeadbob)
        {
            _currentHeadbobOffset = Vector3.zero;
            return;
        }

        Vector3 horizontalVel = new Vector3(player.velocity.x, 0f, player.velocity.z);
        float speed = horizontalVel.magnitude;

        if (player.IsGrounded && speed > 0.1f)
        {
            float speedFactor = speed / player.runSpeed;
            float freq = headbobFrequency * (player.IsRunning ? headbobRunMultiplier : 1f);
            _headbobTimer += Time.deltaTime * freq * Mathf.Clamp01(speedFactor);

            float bobY = Mathf.Sin(_headbobTimer * 2f) * headbobAmplitude;
            float bobX = Mathf.Cos(_headbobTimer) * headbobAmplitude * 0.5f;

            _currentHeadbobOffset = new Vector3(bobX, bobY, 0f);
        }
        else
        {
            _headbobTimer = 0f;
            _currentHeadbobOffset = Vector3.Lerp(
                _currentHeadbobOffset,
                Vector3.zero,
                Time.deltaTime * headbobSmoothing
            );
        }
    }

    private void UpdateJumpLandBob()
    {
        if (!enableJumpLandBob)
        {
            _jumpLandOffsetY = 0f;
            return;
        }

        if (_wasGroundedLastFrame && !player.IsGrounded)
        {
            _jumpLandOffsetY += jumpBobAmount;
        }

        if (!_wasGroundedLastFrame && player.IsGrounded)
        {
            _jumpLandOffsetY -= landBobAmount;
        }

        _jumpLandOffsetY = Mathf.Lerp(
            _jumpLandOffsetY,
            0f,
            Time.deltaTime * jumpLandBobReturnSpeed
        );
    }
    private void UpdateShake()
    {
        if (!enableShake)
        {
            _shakeOffset = Vector3.zero;
            _shakeTimer = 0f;
            return;
        }

        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;

            float t = 1f - Mathf.Clamp01(_shakeTimer / _shakeDuration);
            float damper = 1f - t; 

            float time = Time.time * shakeFrequency + _shakeSeed;
            float offsetX = (Mathf.PerlinNoise(time, 0.0f) - 0.5f) * 2f;
            float offsetY = (Mathf.PerlinNoise(0.0f, time) - 0.5f) * 2f;

            _shakeOffset = new Vector3(offsetX, offsetY, 0f) * _shakeMagnitude * damper;
        }
        else
        {
            _shakeOffset = Vector3.Lerp(_shakeOffset, Vector3.zero, Time.deltaTime * 10f);
        }
    }
    private void UpdateFovKick()
    {
        if (!enableFovKick)
        {
            _targetFov = _baseFov;
            _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFov, Time.deltaTime * fovLerpSpeed);
            return;
        }

        Vector3 horizontalVel = new Vector3(player.velocity.x, 0f, player.velocity.z);
        float speed = horizontalVel.magnitude;

        bool sprintingWithSpeed = player.IsRunning && speed > minSpeedForFovKick;

        if (sprintingWithSpeed)
        {
            _targetFov = _baseFov + sprintFovIncrease;
        }
        else
        {
            _targetFov = _baseFov;
        }

        _cam.fieldOfView = Mathf.Lerp(
            _cam.fieldOfView,
            _targetFov,
            Time.deltaTime * fovLerpSpeed
        );
    }

    public void Shake(float magnitude, float duration)
    {
        if (!enableShake) return;

        _shakeMagnitude = magnitude;
        _shakeDuration = duration;
        _shakeTimer = duration;
    }

    public void Shake()
    {
        Shake(defaultShakeMagnitude, defaultShakeDuration);
    }
}
