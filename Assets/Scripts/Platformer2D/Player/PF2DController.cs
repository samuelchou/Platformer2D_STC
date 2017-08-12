//PF (Platformer) 2D Controller made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/07/23
//NOTE: 2D only.
//Usage: add it to the "player" object you want to control. It will give you basic control, plus funct. working with other "PF-" scripts.
//NOTE: due to the physics of "jump", component rigidbody2D is needed. If no, the script will add one.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PF2DController : MonoBehaviour
{
    //for control
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;
    public KeyCode jumpButton = KeyCode.W;
    public KeyCode dashButton = KeyCode.LeftShift;

    //for moving settings
    public float initialSpeed = 10f;
    public bool allowMovement = true;   //when disabling this, the moving part won't execute. Reassign it when you want to unable move in-game.

    //for jumping settings
    public float initialJumpSpeed = 10f;
    public bool allowJump = true;   //when disabling this, the jump part won't execute. Reassign it when you want to unable jump in-game.
    public bool canOnlyJumpAfterTouchesGround = false;  //if you have a layer "Ground", you can enable this,then object can jump ONLY after touching "Ground"-layered objects. (NOTE: it will also unable the "player-pile-jump". )

    //for dash settings
    public float dashSpeedMultiplier = 3f;
    public bool allowDash = true;   //Just like allowJump and allowMovement...
   
    
    //for state updating
    public Animator animStateMachine;
    public string updateBoolName;
    private bool animSMConditionChecked = false;
    private bool isFreezed = false;
    //once isFreezed, the controller will (for player) be unabled to use.

    //for in-script calculation
    private float currentSpeed = 0f;
    private bool moving = false;
    private Vector3 movingDist;
    //private bool facingRight = true;
    //the "facingRight" bool is used ONLY in dash action. Can be replaced with ASM variable/state.

    private bool iniDashAllowed;
    private bool jumping = false;
    private Rigidbody2D rb;

    //When enabled, accquiring data
    private void OnEnable()
    {
        iniDashAllowed = allowDash;
        rb = GetComponent<Rigidbody2D>();
        if (!GetComponent<PFPlayer>())
        {
            Debug.Log("Player" + name + "doesn't have PFPlayer.cs along with " + GetType().Name + ". "
                + "It seems strange because they're working along, and missing each one might cause errors.");
        }

        //check if animStateMachine have the right bool var.
        if (animStateMachine != null)
            foreach (AnimatorControllerParameter para in animStateMachine.parameters)
            {
                if (para.name == updateBoolName) animSMConditionChecked = true;
            }

    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezed) return;
        if (allowMovement)
        {
            Vector3 headingDirection = Vector3.zero;
            bool lastMoving = moving;
            moving = false;
            //if no press anything, "moving" should remain false

            //detect the button pressed, adding the vector of direction
            //(to headingDirection) (and make moving true)
            CheckMove(rightButton, ref headingDirection, transform.right);
            CheckMove(leftButton, ref headingDirection, -transform.right);

            //if moving, give the object the movingDist (by calculating direction*speed*time)
            if (moving)
            {
                if (lastMoving == false) currentSpeed = initialSpeed;

                movingDist = headingDirection * currentSpeed * Time.deltaTime;
                transform.position += movingDist;

                //velocity horizon set to zero for better control
                if (rb) rb.velocity = new Vector3(0, rb.velocity.y);

                // changing animator state
                if (animSMConditionChecked) animStateMachine.SetBool(updateBoolName, true);

            }
            else
            {
                currentSpeed = 0f;

                // changing animator state
                if (animSMConditionChecked) animStateMachine.SetBool(updateBoolName, false);

            }
            

        }
        if (allowJump)
        {
            if (Input.GetKeyDown(jumpButton) && jumping == false)
            {
                jumping = true;
                //bool "jumping" will be reset only when touched something. Look below.

                rb.velocity = new Vector3(rb.velocity.x, initialSpeed);

                // changing animator state
                if (animSMConditionChecked) animStateMachine.SetBool(updateBoolName, true);
            }
        }
        
    }
    //if it touches something, it should be able to jump again
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canOnlyJumpAfterTouchesGround)
        {
            jumping = false;
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumping = false;
            if (animSMConditionChecked) animStateMachine.SetBool(updateBoolName, false);
        }
    }

    //if it leaves ground, it should be unable to jump (thought as jumping)
    private void OnCollisionExit2D(Collision2D collision)
    {
        jumping = true;
        // Debug.Log("You are falling and can't jump!");
    }

    private void CheckMove(KeyCode keyCode, ref Vector3 deltaPosition, Vector3 directionVector)
    {
        if (Input.GetKey(keyCode))
        {
            moving = true;
            deltaPosition += directionVector;
        }
    }

    public void EnableControl(bool allowMove, bool allowJumping)
    {
        allowMovement = allowMove;
        allowJump = allowJumping;
        allowDash = iniDashAllowed;
    }
    public void EnableControl(bool allowMove, bool allowJump, bool allowDash)
    {
        PF2DController controller = GetComponent<PF2DController>();
        if (!controller)
        {
            Debug.Log("It seems strange that \"player\" " + name + " doesn't have a PFController.cs. "
                + "Thus the advanced funct. for controller from " + GetType().Name + "won't work.");
            return;
        }
        controller.allowMovement = allowMove;
        controller.allowJump = allowJump;
        controller.allowDash = allowDash;

    }
    public void FreezeControl()
    {
        isFreezed = true;
    }
    public void FreezeControl(float freezeTime)
    {
        isFreezed = true;
        StartCoroutine(UnFreezeControl(freezeTime));
    }
    private IEnumerator UnFreezeControl(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        isFreezed = false;
    }
    

}
