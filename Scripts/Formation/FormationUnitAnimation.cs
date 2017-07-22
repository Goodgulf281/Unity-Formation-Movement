using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.t7t.formation
{
    /**
     * This is used on the units to enable their animation to be synced to the path movement. It uses the code from the Coupling Animation and Navigation sample:
     * https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
     * Add this script to the unit which needs to be moved. It requires and Animator component to be present.
     * 
     * Key properties: animationsInUpdate, velocity
     * 
     * Key Methods: Update, StartAnimations, StopAnimations, UpdateAnimations
     * 
     */
    [RequireComponent(typeof(Animator))]
    public class FormationUnitAnimation : MonoBehaviour
    {

        [Header("Animation")]
        // if enables the animations are update in the Update() method.
        // When the unit is moving independently from the grid (for example when its own pathfinding is activated) this should be set to true.
        // In the current implementation of the FormationGrid this is always set to true.
        public bool animationsInUpdate = true;

        // When enabled the animations are randomized in Startanimations to unsync multiple object's animation.
        // This is to prevent each idle state to be in exactly the same state.
        public bool randomizeAnimation = true;

        // This velocity needs to be set by other / outside methods since this class has no concept of velocity.
        // The FormationGrid sets this velocity in its Update() function.
        public Vector3 velocity = Vector3.zero;

        [Header("Sound")]
        /* If true then activate the SoundSource on the grid at certain states*/
        [SerializeField]  protected bool hasSound = true;
        [SerializeField]  protected bool randomStartSound = false;

        /* Cached AudioSource component */
        protected AudioSource audioSource;


        /* Cached Animator component */
        protected Animator anim;

        /* Animations on or off */
        protected bool animations = false;

        /* Smooth Delta for UpdateAnimations*/
        Vector2 smoothDeltaPosition = Vector2.zero;

        // Cache the Animator component.
        private void Awake()
        {
            anim = GetComponent<Animator>();
            if (anim != null)
            {
                //Debug.Log("FormationUnitAnimation.Awake(): animator controller found");
            }
            else Debug.LogError("FormationUnitAnimation.Awake(): no animator controller found");

            if (hasSound)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("FormationUnitAnimation.Awake(): AudioSource component missing");
                    return;
                }
            }
        }


        // Use this for initialization
        void Start()
        {
            //if(hasSound && randomStartSound)
            Random.seed = (int)System.DateTime.Now.Ticks;
        }

        // Update is called once per frame (if enabled).
        void Update()
        {

            if (animationsInUpdate)
            {
                UpdateAnimations(velocity);
            }

        }

        // Enable the animations
        public virtual void StartAnimations()
        {
            animations = true;

            if(randomizeAnimation)
            {
                if(anim)
                {
                    anim.Play(0, -1, Random.value);
                }
            }
        }

        // Disable the animations and stop them immediately
        public virtual void StopAnimations()
        {
            anim.SetBool("move", false);
            anim.SetFloat("velx", 0f);
            anim.SetFloat("vely", 0f);

            anim.Play("Idle", -1, Random.value);

            animations = false;
            SetSoundState(false);
        }

        // See the above URL to the Unity documentation.
        public virtual void UpdateAnimations(Vector3 vlcty)
        {
            Vector3 worldDeltaPosition = vlcty;

            if (anim == null || animations==false) return;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if delta time is safe
            if (Time.deltaTime > 1e-5f)
                vlcty = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = vlcty.magnitude > 0.5f;// && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool("move", shouldMove);
            anim.SetFloat("velx", vlcty.x);
            anim.SetFloat("vely", vlcty.y);

            SetSoundState(shouldMove);

        }
        
        public virtual void SetSoundState(bool state)
        {
            if (hasSound)
            {
                if (state)
                {
                    if (!audioSource.isPlaying)
                    {
                        if (randomStartSound)
                        {
                            audioSource.PlayDelayed(Random.Range(0.0f, 0.2f));
                        }
                        else
                        {
                            audioSource.Play();
                        }
                    }
                    audioSource.mute = false;
                }
                else
                {
                    audioSource.Stop();
                }
            }
        }

    }
}