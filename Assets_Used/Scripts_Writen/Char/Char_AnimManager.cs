using UnityEngine;
using System.Collections;

public class Char_AnimManager : MonoBehaviour {

    private Animator _CharAnimator;                //Character Animation
    internal string _CharAnimation = null;         //Character Animation Name
    private AnimationManagerUI _AnimationManagerUI;         //Character Animation UI Connection
    private string _CharLastAnimation = null;      //Character Last Animation

    CharacterController characterController;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;


    private SkinnedMeshRenderer _CharRenderer_Face;        //Character Skin Mesh Renderer for Face
    private SkinnedMeshRenderer _CharRenderer_Brow;        //Character Skin Mesh Renderer for Eyebrows
    private SkinnedMeshRenderer _CharRenderer_BottomTeeth;        //Character Skin Mesh Renderer for Bottom Teeth
    private SkinnedMeshRenderer _CharRenderer_Tongue;        //Character Tongue Skinned Mesh Renderer
    private SkinnedMeshRenderer _CharRenderer_TopTeeth;      //Character Top Teeth Skinned Mesh Renderer

    private string _EyesLastChangeType = null;      // Character Last Facial Type Update (Eyes)
    private string _EyebrowsLastChangeType = null;  // Character Last Facial Type Update (Eyebrows)
    private string _MouthLastChangeType = null;     // Character Last Facial Type Update (Mouth)

    private float _CharFacial = 0.0f;    //Character Facial Parameter
    private float _CharLastFacial = 0.0f;    //Character Last Facial Parameter
    private bool _CharFacialBool = false;    //Character Facial Parameter Bool
    private bool _CharLastFacialBool = false;    //Character Last Facial Parameter Bool
    //BlendShapeValues
    private float _CharFacial_Eye_L_Happy = 0.0f;
    private float _CharFacial_Eye_R_Happy = 0.0f;
    private float _CharFacial_Eye_L_Closed = 0.0f;
    private float _CharFacial_Eye_R_Closed = 0.0f;
    private float _CharFacial_Eye_L_Wide = 0.0f;
    private float _CharFacial_Eye_R_Wide = 0.0f;

    private float _CharFacial_Mouth_Sad = 0.0f;
    private float _CharFacial_Mouth_Puff = 0.0f;
    private float _CharFacial_Mouth_Smile = 0.0f;

    private float _CharFacial_Eyebrow_L_Up = 0.0f;
    private float _CharFacial_Eyebrow_R_Up = 0.0f;
    private float _CharFacial_Eyebrow_L_Angry = 0.0f;
    private float _CharFacial_Eyebrow_R_Angry = 0.0f;
    private float _CharFacial_Eyebrow_L_Sad = 0.0f;
    private float _CharFacial_Eyebrow_R_Sad = 0.0f;

    private float _CharFacial_Mouth_E = 0.0f;
    private float _CharFacial_Mouth_O = 0.0f;
    private float _CharFacial_Mouth_JawOpen = 0.0f;
    private float _CharFacial_Mouth_Extra01 = 0.0f;
    private float _CharFacial_Mouth_Extra02 = 0.0f;
    private float _CharFacial_Mouth_Extra03 = 0.0f;
    private float _CharFacial_Mouth_BottomTeeth = 0.0f;

    private bool _CharFacial_Mouth_TopTeeth = false;
    private bool _CharFacial_Mouth_Tongue = false;




    private Transform target;
    private Transform cameraTransform;
    public Quaternion cameraOriginalRotationValue;
    public Vector3 cameraOriginalPositionValue;
    public GameObject neck;


    void Start()
    {
        _CharAnimator = this.gameObject.GetComponent<Animator>();
        target = this.gameObject.GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        neck = GameObject.Find("Neck");
        //neck.transform.localEulerAngles = new Vector3(100f, 0f, 0f);
        _AnimationManagerUI = GameObject.Find("AnimationManagerUI").GetComponent<AnimationManagerUI>();
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        cameraOriginalRotationValue = cameraTransform.rotation;
        cameraOriginalPositionValue = new Vector3(0.85f, 2.80f, -4.38f);
        Transform[] CharChildren = GetComponentsInChildren<Transform>();

        foreach (Transform t in CharChildren)
        {
            if (t.name == "face")
                _CharRenderer_Face = t.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (t.name == "brow")
                _CharRenderer_Brow = t.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (t.name == "BottomTeeth")
                _CharRenderer_BottomTeeth = t.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (t.name == "tongue")
                _CharRenderer_Tongue = t.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (t.name == "TopTeeth")
                _CharRenderer_TopTeeth = t.gameObject.GetComponent<SkinnedMeshRenderer>();
        }
        _CharRenderer_Tongue.enabled = false;
        _CharRenderer_TopTeeth.enabled = false;
    }


    void GetAnimation()
    {
        //Record Last Animation
        _CharLastAnimation = _CharAnimation;

        if (_CharAnimation == null)
            _CharAnimation = "idle";

        else
        {
            //Set Animation Parameter
            _CharAnimation = _AnimationManagerUI._Animation;
            //_CharAnimation = "hit01";
        }
    }

    void SetAllAnimationFlagsToFalse()
    {
        _CharAnimator.SetBool("param_idletowalk", false);
        _CharAnimator.SetBool("param_idletorunning", false);
        _CharAnimator.SetBool("param_idletojump", false);
        _CharAnimator.SetBool("param_idletowinpose", false);
        _CharAnimator.SetBool("param_idletoko_big", false);
        _CharAnimator.SetBool("param_idletodamage", false);
        _CharAnimator.SetBool("param_idletohit01", false);
        _CharAnimator.SetBool("param_idletohit02", false);
        _CharAnimator.SetBool("param_idletohit03", false);
    }


    void SetAnimation()
    {
        SetAllAnimationFlagsToFalse();

        //IDLE
        if (_CharAnimation == "idle")
        {
            _CharAnimator.SetBool("param_toidle", true);
        }

        //WALK
        else if (_CharAnimation == "walk")
        {
            _CharAnimator.SetBool("param_idletowalk", true);
        }

        //RUN
        else if (_CharAnimation == "running")
        {
            //if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s"))
           
                _CharAnimator.SetBool("param_runningtojump", true);
                _CharAnimator.SetBool("param_idletorunning", true);
      
        }

        //JUMP
        else if (_CharAnimation == "jump")
        {
            _CharAnimator.SetBool("param_idletojump", true);
        }

        //WIN POSE
        else if (_CharAnimation == "winpose")
        {
            _CharAnimator.SetBool("param_idletowinpose", true);
        }

        //KO
        else if (_CharAnimation == "ko_big")
        {
            _CharAnimator.SetBool("param_idletoko_big", true);
        }

        //DAMAGE
        else if (_CharAnimation == "damage")
        {
            _CharAnimator.SetBool("param_idletodamage", true);
        }

        //HIT 1
        else if (_CharAnimation == "hit01")
        {
            _CharAnimator.SetBool("param_idletohit01", true);
        }

        //HIT 2
        else if (_CharAnimation == "hit02")
        {
            _CharAnimator.SetBool("param_idletohit02", true);
        }

        //HIT 3
        else if (_CharAnimation == "hit03")
        {
            _CharAnimator.SetBool("param_idletohit03", true);
        }
    }

    void ReturnToIdle()
    {
        if (_CharAnimator.GetCurrentAnimatorStateInfo(0).IsName(_CharAnimation))
        {
            if (
                _CharAnimation != "walk" &&
                _CharAnimation != "running" &&
                _CharAnimation != "ko_big" &&
                _CharAnimation != "winpose"
                )
            {
                SetAllAnimationFlagsToFalse();
                _CharAnimator.SetBool("param_toidle", true);
            }
        }
    }


    void SetFacial()
    {

        //Override the Animator
        _CharRenderer_Face.SetBlendShapeWeight(0, _CharFacial_Eye_L_Happy);
        _CharRenderer_Face.SetBlendShapeWeight(1, _CharFacial_Eye_R_Happy);
        _CharRenderer_Face.SetBlendShapeWeight(4, _CharFacial_Eye_L_Closed);
        _CharRenderer_Face.SetBlendShapeWeight(5, _CharFacial_Eye_R_Closed);
        _CharRenderer_Face.SetBlendShapeWeight(2, _CharFacial_Eye_L_Wide);
        _CharRenderer_Face.SetBlendShapeWeight(3, _CharFacial_Eye_R_Wide);
        
        _CharRenderer_Brow.SetBlendShapeWeight(0, _CharFacial_Eyebrow_L_Up);
        _CharRenderer_Brow.SetBlendShapeWeight(1, _CharFacial_Eyebrow_R_Up);
        _CharRenderer_Brow.SetBlendShapeWeight(2, _CharFacial_Eyebrow_L_Angry);
        _CharRenderer_Brow.SetBlendShapeWeight(3, _CharFacial_Eyebrow_R_Angry);
        _CharRenderer_Brow.SetBlendShapeWeight(4, _CharFacial_Eyebrow_L_Sad);
        _CharRenderer_Brow.SetBlendShapeWeight(5, _CharFacial_Eyebrow_R_Sad);

        _CharRenderer_Face.SetBlendShapeWeight(6, _CharFacial_Mouth_E);
        _CharRenderer_Face.SetBlendShapeWeight(8, _CharFacial_Mouth_O);
        _CharRenderer_Face.SetBlendShapeWeight(7, _CharFacial_Mouth_JawOpen);
        _CharRenderer_Face.SetBlendShapeWeight(12, _CharFacial_Mouth_Extra01);
        _CharRenderer_Face.SetBlendShapeWeight(13, _CharFacial_Mouth_Extra02);
        _CharRenderer_Face.SetBlendShapeWeight(14, _CharFacial_Mouth_Extra03);

        _CharRenderer_Face.SetBlendShapeWeight(9, _CharFacial_Mouth_Sad);
        _CharRenderer_Face.SetBlendShapeWeight(10, _CharFacial_Mouth_Puff);
        _CharRenderer_Face.SetBlendShapeWeight(11, _CharFacial_Mouth_Smile);

        if(_CharRenderer_BottomTeeth.isVisible)
            _CharRenderer_BottomTeeth.SetBlendShapeWeight(0, _CharFacial_Mouth_BottomTeeth);





    string _GeneralChangeType = _AnimationManagerUI._GeneralChangeType;
        _CharLastFacial = _CharFacial;
        _CharFacial = _AnimationManagerUI._FacialValue;
        _CharLastFacialBool = _CharFacialBool;
        _CharFacialBool = _AnimationManagerUI._FacialValueBool;

        if (_GeneralChangeType == null)
        {
            return;
        }

        else if (_GeneralChangeType == "eyes")
        {
            string _EyesChangeType = _AnimationManagerUI._EyesChangeType;
            if (_EyesChangeType == null)
                return;

            if (_EyesChangeType == _EyesLastChangeType && _CharFacial == _CharLastFacial)
                return;

            else if (_EyesChangeType == "happyL")
            {
                _EyesLastChangeType = _EyesChangeType;
                _CharFacial_Eye_L_Happy = _CharFacial;
                _CharRenderer_Face.SetBlendShapeWeight(0, _CharFacial);
            }
            else if (_EyesChangeType == "happyR")
            {
                _EyesLastChangeType = _EyesChangeType;
                _CharFacial_Eye_R_Happy = _CharFacial;
                _CharRenderer_Face.SetBlendShapeWeight(1, _CharFacial);
            }
            else if (_EyesChangeType == "closedL")
            {
                _CharFacial_Eye_L_Closed = _CharFacial;
                _EyesLastChangeType = _EyesChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(4, _CharFacial);

            }
            else if (_EyesChangeType == "closedR")
            {
                _CharFacial_Eye_R_Closed = _CharFacial;
                _EyesLastChangeType = _EyesChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(5, _CharFacial);

            }
            else if (_EyesChangeType == "wideL")
            {
                _CharFacial_Eye_L_Wide = _CharFacial;
                _EyesLastChangeType = _EyesChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(2, _CharFacial);

            }
            else if (_EyesChangeType == "wideR")
            {
                _CharFacial_Eye_R_Wide = _CharFacial;
                _EyesLastChangeType = _EyesChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(3, _CharFacial);
            }
        }



        else if (_GeneralChangeType == "eyebrows")
        {
            string _EyebrowsChangeType = _AnimationManagerUI._EyebrowsChangeType;
            if (_EyebrowsChangeType == null)
                return;

            if (_EyebrowsChangeType == _EyebrowsLastChangeType && _CharFacial == _CharLastFacial)
                return;

            else if (_EyebrowsChangeType == "upL")
            {
                _CharFacial_Eyebrow_L_Up = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(0, _CharFacial);
            }

            else if (_EyebrowsChangeType == "upR")
            {
                _CharFacial_Eyebrow_R_Up = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(1, _CharFacial);
            }

            else if (_EyebrowsChangeType == "angerL")
            {
                _CharFacial_Eyebrow_L_Angry = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(2, _CharFacial);
            }

            else if (_EyebrowsChangeType == "angerR")
            {
                _CharFacial_Eyebrow_R_Angry = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(3, _CharFacial);
            }

            else if (_EyebrowsChangeType == "sadL")
            {
                _CharFacial_Eyebrow_L_Sad = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(4, _CharFacial);
            }

            else if (_EyebrowsChangeType == "sadR")
            {
                _CharFacial_Eyebrow_R_Sad = _CharFacial;
                _EyebrowsLastChangeType = _EyebrowsChangeType;
                _CharRenderer_Brow.SetBlendShapeWeight(5, _CharFacial);
            }
        }

        else if (_GeneralChangeType == "mouth")
        {
            string _MouthChangeType = _AnimationManagerUI._MouthChangeType;
            if (_MouthChangeType == null)
                return;

            if (_MouthChangeType == _MouthLastChangeType && 
                _CharFacial == _CharLastFacial &&
                _CharFacialBool == _CharLastFacialBool)
                return;

            else if (_MouthChangeType == "mouthE")
            {
                _CharFacial_Mouth_E = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(6, _CharFacial);
            }

            else if (_MouthChangeType == "mouthO")
            {
                _CharFacial_Mouth_O = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(8, _CharFacial);
            }

            else if (_MouthChangeType == "mouthJawOpen")
            {
                _CharFacial_Mouth_JawOpen = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(7, _CharFacial);
            }

            else if (_MouthChangeType == "mouthExtra01")
            {
                _CharFacial_Mouth_Extra01 = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(12, _CharFacial);
            }

            else if (_MouthChangeType == "mouthExtra02")
            {
                _CharFacial_Mouth_Extra02 = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(13, _CharFacial);
            }

            else if (_MouthChangeType == "mouthExtra03")
            {
                _CharFacial_Mouth_Extra03 = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(14, _CharFacial);
            }

            else if (_MouthChangeType == "sad")
            {
                _CharFacial_Mouth_Sad = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(9, _CharFacial);

            }
            else if (_MouthChangeType == "puff")
            {
                _CharFacial_Mouth_Puff = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(10, _CharFacial);
            }
            else if (_MouthChangeType == "smile")
            {
                _CharFacial_Mouth_Smile = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_Face.SetBlendShapeWeight(11, _CharFacial);
            }

            else if (_MouthChangeType == "mouthBottomTeeth")
            {
                _CharFacial_Mouth_BottomTeeth = _CharFacial;
                _MouthLastChangeType = _MouthChangeType;
                _CharRenderer_BottomTeeth.SetBlendShapeWeight(0, _CharFacial);
            }

            else if (_MouthChangeType == "mouthTopTeeth")
            {
                _CharFacial_Mouth_TopTeeth = _CharFacialBool;
                Debug.Log(_CharFacialBool);
                _MouthLastChangeType = _MouthChangeType;
                if (_CharFacialBool == false)
                    _CharRenderer_TopTeeth.enabled = _CharFacialBool;
                else
                    _CharRenderer_TopTeeth.enabled = true;
            }

            else if (_MouthChangeType == "mouthTongue")
            {
                _CharFacial_Mouth_Tongue = _CharFacialBool;
                _MouthLastChangeType = _MouthChangeType;
                if (_CharFacialBool == false)
                    _CharRenderer_Tongue.enabled = _CharFacialBool;
                else
                    _CharRenderer_Tongue.enabled = true;
            }
        }
    }

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    

    public float rotateSpd = 15.0f;
    public float movementSpd = 2.0f;
    public float sprintSpd = 2.0f;
    public bool saveCameraPosition = true;

    float lerpTime = 1f;
    float currentLerpTime;

    
    public float lastValue = 100f;
    IEnumerator BackToPlayer(float duration)
    {
       
        for (float t = 0f; t < duration; t += Time.deltaTime*2)
        {
            if (cameraTransform.localEulerAngles.y > 180)
            {
                lastValue = cameraTransform.localEulerAngles.y;
              
                cameraTransform.RotateAround(target.transform.position, Vector3.up * t, 150 * Time.deltaTime);
                neck.transform.localEulerAngles = new Vector3(-cameraTransform.localEulerAngles.y, 0f);
                if (lastValue < 2)
                {
                    break;
                }

            }
            if(cameraTransform.localEulerAngles.y < 180)
            {
                lastValue = cameraTransform.localEulerAngles.y;
 
                cameraTransform.RotateAround(target.transform.position, Vector3.up * -t, 150 * Time.deltaTime);
                neck.transform.localEulerAngles = new Vector3(-cameraTransform.localEulerAngles.y, 0f);
                if (lastValue<2)
                {
                    break;
                }


            }
           
         
            yield return 0;
        }
        yield return 0;
    }


    void Update ()
    {

        Vector3 movement = Vector3.zero;

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float t = currentLerpTime / lerpTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);

        float horInput = Input.GetAxis("Horizontal") * movementSpd;
        float vertInput = Input.GetAxis("Vertical") * movementSpd;


        
        if(!characterController.isGrounded)
        {
            Debug.Log("I'm in the air!");
            _AnimationManagerUI.SetAnimation_Jump();
            if (Input.GetMouseButton(1))
            {
                float h = horizontalSpeed * Input.GetAxis("Mouse X");
                transform.Rotate(0, h, 0);
            }
        }
        else
        {
            if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s"))
            {
                Debug.Log("Move!");
                if(!saveCameraPosition)
                {
                    //cameraTransform.rotation = cameraOriginalRotationValue
                    /*cameraTransform.localPosition = new Vector3(0.85f, 2.78f, -4.38f); */
                    //cameraTransform.localPosition = Vector3.Lerp(cameraTransform.position, new Vector3(0.85f, 2.78f, -4.38f), t);
                    StartCoroutine(BackToPlayer(5f));
                    saveCameraPosition = true;
                    Debug.Log("RESTORE CAMERA");

                }

                if (characterController.isGrounded)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                      
                        _AnimationManagerUI.SetAnimation_Run();
                        if (Input.GetMouseButton(1))
                        {
                           
                        
                            float h = horizontalSpeed * Input.GetAxis("Mouse X");
                            transform.Rotate(0, h, 0);
                        }
                    }
                    else
                    {
                    
                        _AnimationManagerUI.SetAnimation_Walk();
                        if (Input.GetMouseButton(1))
                        {
                            float h = horizontalSpeed * Input.GetAxis("Mouse X");
                            transform.Rotate(0, h, 0);
                        }
                    }

                }
            }
            else
            {
                if (characterController.isGrounded&&!Input.GetKey("space"))
                    _AnimationManagerUI.SetAnimation_Idle();

                if (Input.GetMouseButton(0))
                {
                    if(saveCameraPosition)
                    {
                        cameraOriginalPositionValue = cameraTransform.position;
                        cameraOriginalRotationValue = cameraTransform.rotation;
                        saveCameraPosition = false;
                        Debug.Log("SAVED CAMERA");
                    }
                    float h = horizontalSpeed * Input.GetAxis("Mouse X");

                    float angle = cameraTransform.localEulerAngles.y;
                    Debug.Log(angle);
                  
                    //neck.transform.localEulerAngles = new Vector3(angle, Time.deltaTime);
                    



                    //neck.transform.localEulerAngles.Set(cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.x, 0f);
                    cameraTransform.RotateAround(target.transform.position, Vector3.up * h, 300 * Time.deltaTime);
                   
                   
                    
                }
                
            }
        }
   

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly 
            if (Input.GetButton("Jump"))
            {
                moveDirection.x = movement.x;
                moveDirection.z = moveDirection.z;
                moveDirection.y = jumpSpeed;
            }

          
        }


        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * movementSpd;
            movement.z = vertInput * movementSpd;

            movement = Vector3.ClampMagnitude(movement, movementSpd);

            Quaternion temp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);

            movement = target.TransformDirection(movement);

            target.rotation = temp;

            Quaternion direction = Quaternion.LookRotation(movement);

        }



        movement *= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterController.Move(sprintSpd*movement);
        }
        else
        {
            characterController.Move(movement);
        }
           

        moveDirection.y -= gravity * Time.deltaTime;


        characterController.Move(moveDirection * Time.deltaTime);
      








        //Get Animation from UI
        GetAnimation();

        //Set New Animation
        if (_CharLastAnimation != _CharAnimation)
            SetAnimation();
        else
        {
            //Debug.Log("Return to Idle");
            //ReturnToIdle();
        }

    }

    void LateUpdate()
    {
        //Set Facial
        //SetFacial();
    }
}
