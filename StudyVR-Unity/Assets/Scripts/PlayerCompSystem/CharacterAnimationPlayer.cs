using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationPlayer : MonoBehaviour
{
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;
    public Vector3 ScaleOffset = Vector3.one;

    [SerializeField][HideInInspector] public float PlayerFramePerSecond = 60;

    private Animator animator;
    private AnimatorStateInfo animatorStateInfo;

    private float AnimationLength = 0f; 

    public bool isPlaying = false;

    public bool isPlayingAudio = false;
    [SerializeField][HideInInspector] public AudioClip PlayerAudio;
    private AudioSource PlayerAudioSource;
    [HideInInspector] public float AudioPlayOffset = 0f;

    private bool isAudioStarted = false;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Please setup animation first!");
            isPlaying = false;
            return;
        }

        if (isPlayingAudio)
        {
            if (PlayerAudio != null)
            {
                PlayerAudioSource = this.gameObject.AddComponent<AudioSource>();
                PlayerAudioSource.clip = PlayerAudio;
                PlayerAudioSource.loop = false;

                PlayerAudioSource.spatialBlend = 1f;
                PlayerAudioSource.rolloffMode = AudioRolloffMode.Linear;
                PlayerAudioSource.maxDistance = 2.5f;
            }
            else
            {
                Debug.LogError("Please setup audio first!");
                isPlayingAudio = false;
            }
        }

        if(!isPlaying)
        {
            animator.speed = 0f;
        }
        
    }

    private void Start()
    {
        if(animator != null)
        {
            this.transform.position += PositionOffset;
            this.transform.eulerAngles += RotationOffset;
            this.transform.localScale = ScaleOffset;

            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimationLength = animatorStateInfo.length;
        }
    }

    private void Update()
    {
        if(isPlaying)
        {
            float CurrentAnimationNTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (CurrentAnimationNTime >= 1f)
            {
                OnAnimationReplay();
            }

            if (isPlayingAudio)
            {
                //if (Mathf.FloorToInt(PlayerAudioSource.time / PlayerAudio.length * (FrameCount - 1)) != CurrentFrame)

                if (!isAudioStarted && (CurrentAnimationNTime * AnimationLength) >= AudioPlayOffset)
                {
                    PlayerAudioSource.Play();
                    PlayerAudioSource.time = 0f;
                    isAudioStarted = true;
                }
            }
        }
    }

    [ContextMenu("Apply Player Transform Offset")]
    public void ApplyOffset()
    {
        PositionOffset = this.transform.position;
        RotationOffset = this.transform.eulerAngles;
        ScaleOffset = this.transform.localScale;

        Debug.Log("Offset Applied");
    }

    public void OnAudioOffsetChanged(float dOffset)
    {
        if(isAudioStarted)
        {
            PlayerAudioSource.time += -dOffset;
        }
    }

    private void OnAnimationReplay()
    {
        if (isPlayingAudio) 
        {
            PlayerAudioSource.Stop();
            isAudioStarted = false;
        }
        animator.Play(animatorStateInfo.fullPathHash, 0, 0f);
    }
}
