import System.Collections;
import System.Collections.Generic;
import UnityEngine;

public class PlayerMoves extends MonoBehaviour {

	private var m_Cam : Transform;                  // A reference to the main camera in the scenes transform
    private var m_CamForward : Vector3;             // The current forward direction of the camera
    private var m_Move : Vector3;
    private var m_Jump : boolean;

    @SerializeField var m_MovingTurnSpeed :float = 360;
    @SerializeField var m_StationaryTurnSpeed : float = 180;
    @SerializeField var m_JumpPower : float = 12f;
    @SerializeField var m_airSpeed : float = 7f;
    @Range(1, 4)
    @SerializeField var m_GravityMultiplier : float = 2f;
    @SerializeField var m_GroundCheckDistance : float = 0.2f;

    private var m_Rigidbody : Rigidbody;
    //Animator m_Animator;
    private var m_Animable : CharacterAnimations;
    private var m_IsGrounded : boolean;
    private var m_OrigGroundCheckDistance : float;
    private var k_Half : float = 0.5f;
    private var m_TurnAmount : float;
    private var m_ForwardAmount : float;
    private var m_GroundNormal : Vector3;
    private var m_Crouching : boolean;
    private var isCarrying : boolean = false;
    private var isDead : boolean = false;
    private var canGather : boolean = false;
    private var enemyLayerMask : int;

    @HideInInspector
    var animSpeed : Vector2;

    function Awake ()
    {
        //m_Animator = GetComponent.<Animator>();
        m_Rigidbody = GetComponent.<Rigidbody>();
        m_Animable = GetComponent.<CharacterAnimations>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        enemyLayerMask = 1 << 10;
    }


	// Use this for initialization
	function Start () {
		// get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
	}
	
	// Update is called once per frame
	function Update () {
		if (Input.GetButtonDown("X") && canGather)
        {
            m_Animable.Gather();
            StartCoroutine("GatherCD");
        }
	}

    function GatherCD ()
    {
        canGather = false;
        yield WaitForSeconds(1);
        m_Animable.StopGathering();
        m_Animable.Grab();
    }

	function FixedUpdate()
    {

        if (isDead)
        {
            return;
        }
        // read inputs
        var h :float = Input.GetAxis("LeftAnalogHorizontal");
        var v : float = Input.GetAxis("LeftAnalogVertical");
        m_Jump = Input.GetButtonDown("A");

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v*Vector3.forward + h*Vector3.right;
        }

        // TODO : On garde une sorte de sprint ??

        //#region Le susmentioné sprint

#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        //#endregion

        // pass all parameters to the character control script
        Move(m_Move, m_Jump);
        m_Jump = false;

        //m_Animable.SetGrounded(m_IsGrounded);
    }

    function ToggleCarrying ()
    {
        isCarrying = !isCarrying;
    }

    function TriggerDeath ()
    {
        isDead = true;
    }

	function ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		var turnSpeed : float = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}

	function Move(move : Vector3, jump : boolean)
	{
		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

		ApplyExtraTurnRotation();

        animSpeed = new Vector2(move.x, move.z);

        m_Animable.SetAnimSpeed(move.x,move.z);

		// control and velocity handling is different when grounded and airborne:
		if (m_IsGrounded)
		{
			HandleGroundedMovement(jump);
		}
		else
		{
			HandleAirborneMovement(move);
		}
	}

	function CheckGroundStatus()
	{
		var hitInfo : RaycastHit;
#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
            //m_Animable.SetGrounded(true);
            m_IsGrounded = true;
            m_Animable.Land();
        }
		else
		{
            //m_Animable.SetGrounded(false);
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
		}
	}

	function HandleAirborneMovement(moves : Vector3)
	{
		// apply extra gravity from multiplier:
		var extraGravityForce : Vector3;
        extraGravityForce = new Vector3(m_Move.x * m_airSpeed, Physics.gravity.y * m_GravityMultiplier, m_Move.z * m_airSpeed);
        extraGravityForce -=  Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}


	function HandleGroundedMovement(jumpMove : boolean)
	{
		// check whether conditions are right to allow a jump:
		if (jumpMove && m_IsGrounded && !isCarrying /*&& m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")*/)
		{
			// jump!
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            //m_Animable.SetGrounded(false);
            m_IsGrounded = false;
            m_Animable.Jump();
            m_GroundCheckDistance = 0.2f;
		}
	}

    function SetCanGather ()
    {
        canGather = true;

    }

    function SetCannotGather ()
    {
        canGather = false;
    }
}
