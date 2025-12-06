using System.Collections;
using UnityEngine;

public class CloneSpawnner : MonoBehaviour
{
    public PlayerInputRecorder recorder;
    public GameObject clonePrefab;
    public Vector3 cloneSpawnPosition;
    public Quaternion cloneSpawnRotation;
    public bool canSpawnClone = true;
    [SerializeField] float maxRecordingTime = 3f;
    [SerializeField] float cloneSpawnDelay = 0.5f;

    private IPlayerInput _input;
    bool recording = false;
    bool cloneSpawnned = false;
    float recordingTime;
    GameObject clone;

    void Start()
    {
        _input = gameObject.GetComponent<PlayerController>()._input;
    }

    void Update()
    {
        if (!canSpawnClone) return;
        if (gameObject.GetComponent<PlayerController>().gameObjectController != PlayerController.Controller.Player) return;
        if (cloneSpawnned) return;
        if (recording)
        {
            if((Time.time - recordingTime >= maxRecordingTime) || _input.recordKeyPressed)
            {
                recorder.StopRecording();
                recording = false;
                Invoke(nameof(SpawnClone),cloneSpawnDelay);
            }
        }
        else
        {
            if(_input.recordKeyPressed)
            {
                recordingTime = Time.time;
                cloneSpawnPosition = transform.position;
                cloneSpawnRotation = transform.rotation;
                recorder.StartRecording();
                recording = true;
            }
        }
    }

    public void SpawnClone()
    {
        if (recorder.recordedFrames.Count == 0)
        {
            Debug.LogWarning("No recording to play.");
            return;
        }

        Vector3 pos = cloneSpawnPosition;
        Quaternion rot = cloneSpawnRotation;

        clone = Instantiate(clonePrefab, pos, rot);
        cloneSpawnned = true;

        PlaybackInput playback = clone.GetComponentInChildren<PlaybackInput>();
        if (playback == null)
        {
            Debug.LogError("Ghost prefab has no PlaybackInput!");
            return;
        }

        playback.BeginPlayback(recorder.recordedFrames);
        Invoke(nameof(DestroyClone), maxRecordingTime + 1f);
    }


    void DestroyClone()
    {
        cloneSpawnned = false;
        Destroy(clone.gameObject);
    }
    
}
