#pragma strict

private var animator : Animator;

function Awake (){
	animator = GetComponent.<Animator>();
}

function Start ()
{
    animator.applyRootMotion = true;
}

function Update()
{

}

function Die()
{
    animator.SetTrigger("Death");
}

function Jump()
{
    animator.applyRootMotion = false;
    animator.SetBool("Jump", true);
}

function Land()
{
    animator.applyRootMotion = true;
    animator.SetBool("Jump", false);
}

function SetAnimSpeed (hSpeed : float, vSpeed : float)
{
    var animSpeed : float = Mathf.Clamp(Mathf.Abs(hSpeed) + Mathf.Abs(vSpeed), 0f, 1f);
    animator.SetFloat("Speed", animSpeed);
}