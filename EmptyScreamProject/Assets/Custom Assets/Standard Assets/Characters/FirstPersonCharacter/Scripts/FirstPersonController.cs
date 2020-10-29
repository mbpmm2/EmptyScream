using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        public delegate void OnFPSAction();
        static public OnFPSAction OnFPSJumpStart;
        static public OnFPSAction OnFPSJumpEnd;
        static public OnFPSAction OnFPSCrouchStart;
        static public OnFPSAction OnFPSCrouchEnd;

        // public Animator animator;
        public float axisSpeedMultiplier;
        public float finalAxisValueX;
        public float finalAxisValueY;
        public bool isIdle;
        public bool isRunning;
        public bool m_IsWalking;
        public float m_WalkSpeed;
        public float m_RunSpeed;
        [Range(0f, 2f)] public float m_RunstepLenghten;
        public float m_JumpSpeed;
        public float m_StickToGroundForce;
        public float m_GravityMultiplier;
        public MouseLook m_MouseLook;
        public bool m_UseFovKick;
        public FOVKick m_FovKick = new FOVKick();
        public bool m_UseHeadBob;
        public CurveControlledBob m_HeadBob = new CurveControlledBob();
        public float m_BobInterval;
        public LerpControlledBob m_JumpBob = new LerpControlledBob();
        public float m_StepInterval;
        public AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        public AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        public AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        public bool m_Jumping;
        private AudioSource m_AudioSource;

        //Crouch

        public bool m_IsCrouching;
        public float originalHeight;
        public float crouchHeight=0.5f;
        public float t = 0;
        public float crouchSpeed;
        public float crouchMovSpeed;
        public LayerMask layer;

        public float originalSpeed;
        public float originalRunSpeed;

        // Use this for initialization
        private void Start()
        {
            //animator = transform.GetChild(0).GetComponent<Animator>();
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_BobInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            originalHeight = m_CharacterController.height;

            m_WalkSpeed = 3;
            m_RunSpeed = 4;
            originalSpeed = m_WalkSpeed;
            originalRunSpeed = m_RunSpeed;
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump && CanStandUp())
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (CanStandUp())
                {
                    m_IsCrouching = !m_IsCrouching;

                    if(m_IsCrouching)
                    {
                        if (OnFPSCrouchStart != null)
                        {
                            OnFPSCrouchStart();
                        }
                    }
                    else
                    {
                        if (OnFPSCrouchEnd != null)
                        {
                            OnFPSCrouchEnd();
                        }
                    }
                    t = 0;
                }
                
            }

            CheckCrouch();
            
            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            CharacterUpdate();
        }

        private void CheckCrouch()
        {
            if (m_IsCrouching)
            {
                t += Time.deltaTime;
                m_CharacterController.height = Mathf.Lerp(originalHeight, crouchHeight, t * crouchSpeed);
                m_WalkSpeed = crouchMovSpeed;
            }
            else
            {
                t += Time.deltaTime;
                m_CharacterController.height = Mathf.Lerp(crouchHeight, originalHeight, t * crouchSpeed);
                m_WalkSpeed = originalSpeed;
            }
        }

        private bool CanStandUp()
        {
            if (Physics.CheckCapsule(transform.position + Vector3.up * (crouchHeight),transform.position + Vector3.up * (originalHeight - m_CharacterController.radius),
                 m_CharacterController.radius - Physics.defaultContactOffset, layer, QueryTriggerInteraction.Ignore))
                return false;
            else
                return true; // can  stand up
        }

        private void PlayLandingSound()
        {
            //Jump delegate
            if (OnFPSJumpEnd != null)
            {
                OnFPSJumpEnd();
            }
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void CharacterUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    //Jump delegate
                    if(OnFPSJumpStart != null)
                    {
                        OnFPSJumpStart();
                        Debug.Log("JUMPING");
                    }
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier* Time.deltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir* Time.deltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

           // m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            AkSoundEngine.PostEvent("player_jump", gameObject);
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            
            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0

            AkSoundEngine.PostEvent("player_step", gameObject);
            //int n = Random.Range(1, m_FootstepSounds.Length);
            //m_AudioSource.clip = m_FootstepSounds[n];
            //m_AudioSource.PlayOneShot(m_AudioSource.clip);
            //// move picked sound to index 0 so it's not picked next time
            //m_FootstepSounds[n] = m_FootstepSounds[0];
            //m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            speed = 0;
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal") * axisSpeedMultiplier;
            float vertical = CrossPlatformInputManager.GetAxis("Vertical") * axisSpeedMultiplier;
            float pureHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float pureVertical = CrossPlatformInputManager.GetAxis("Vertical");
            finalAxisValueX = pureHorizontal;
            finalAxisValueY = pureVertical;

            if(horizontal == 0 && vertical == 0)
            {
                isIdle = true;
            }
            else
            {
                isIdle = false;
            }

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running

            if (m_CharacterController.isGrounded)
            {
                if (!isIdle)
                {
                    if(!m_IsCrouching)
                    {
                        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
                        isRunning = Input.GetKey(KeyCode.LeftShift);
                    }
                    else
                    {
                        m_IsWalking = true;
                        isRunning = false;
                    }
                }
                else
                {
                    m_IsWalking = false;
                    isRunning = false;
                }
            }
            
            if(m_IsCrouching && m_IsWalking)
            {
                if (m_CharacterController.isGrounded)
                {
                    m_RunSpeed = crouchMovSpeed;
                }
                
            }
            else
            {
                if (m_CharacterController.isGrounded)
                {
                    m_RunSpeed = originalRunSpeed;
                }
                
            }
#endif
            // set the desired speed to be walking or running
            if(m_IsWalking)
            {
                speed = m_WalkSpeed;
            }
            else if(isRunning)
            {
                speed = m_RunSpeed;
            }
            //speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            //if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            //{
            //    StopAllCoroutines();
            //    StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            //}
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
