using Assets.angryDad.Scripts.Player.ScareLogic;
using Assets.angryDad.Scripts.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneScareDetector : MonoBehaviour
{
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private float volumeThreshold = 0.1f;

    private AudioSource audioSource;
    private AudioClip micClip;
    private MicrophoneScareService scareService;

    private void Start()
    {
        string micName = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;
        if (micName == null)
        {
            Debug.LogError("🎙️ Микрофон не найден");
            return;
        }

        micClip = Microphone.Start(micName, true, 1, 44100);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = micClip;
        audioSource.loop = true;
        audioSource.Play();

        scareService = new MicrophoneScareService(maxRadius, volumeThreshold);
        ServiceLocator.Register(scareService);
    }

    private void Update()
    {
        if (micClip == null)
            return;

        float loudness = GetMicVolume();
        scareService.UpdateScare(loudness, transform.position);
       // Debug.Log($"🎙 Loudness: {loudness}, Threshold: {volumeThreshold}");
        if (loudness > volumeThreshold)
        {
           
           Collider[] colliders = Physics.OverlapSphere(transform.position, scareService.CurrentScareRadius);
            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out ChildController child))
                {
                    GameEvents.ScareChildByMic(child);
                }
            }
        }
    }

    private float GetMicVolume()
    {
        float[] data = new float[256];
        audioSource.GetOutputData(data, 0);
        float sum = 0f;
        foreach (var val in data)
            sum += val * val;

        return Mathf.Sqrt(sum / data.Length);
    }

    private void OnDrawGizmosSelected()
    {
        if (scareService != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, scareService.CurrentScareRadius);
        }
    }
}