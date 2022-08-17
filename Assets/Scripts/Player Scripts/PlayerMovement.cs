using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState{
    walk,
    attack,
    attack2,
    attack3,
    interact,
    stagger,
    idle
}
public class PlayerMovement : Photon.MonoBehaviour
{
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public bool facingRight= true;
    public SpriteRenderer sr;
    PhotonView photonView;
    public bool isGrounded = false;
    public GameObject PlayerCamera;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;


    public Text TextName;
    [SerializeField] private Transform playerTr = null;


    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        // myBody = this.GetComponent<Rigidbody2D>();
        // myBody.freezeRotation = true;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    void FixedUpdate()
    {
            if (photonView.isMine)
            {
                PlayerCamera.SetActive(true);
/*                CheckInput();*/
                TextName.text = PhotonNetwork.playerName;
            }
            else
            {
                TextName.text = photonView.owner.name;
                TextName.color = Color.cyan;
            }

        /* if(TextName != null)
         {
             TextName.transform.LookAt(Camera.main.transform.position);
             TextName.transform.Rotate(0,180,0);
         }*/
    }
    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }
    }

    public void RaiseItem()
    {
        if(currentState != PlayerState.idle)
        {
            animator.SetBool("receive item", true);
            currentState = PlayerState.idle;
            receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
        }
        else
        {
            animator.SetBool("receive item", false);
            currentState = PlayerState.idle;
            receivedItemSprite.sprite = null;
        }
    }
    private void CheckInput()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(change);
        if (change.x > 0 && !facingRight)
        {
            photonView.RPC("Flip", PhotonTargets.AllBuffered);
        }
        if (change.x < 0 && facingRight)
        {
            photonView.RPC("Flip", PhotonTargets.AllBuffered);
        }
        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            // Debug.Log("q is pressed");
            StartCoroutine(AttackCo());
        }
        if (Input.GetButtonDown("Attack2") && currentState != PlayerState.attack2
           && currentState != PlayerState.stagger)
        {
/*            Debug.Log("e is pressed");*/
            StartCoroutine(AttackCo2());
        }
        if (Input.GetButtonDown("Attack3") && currentState != PlayerState.attack3
           && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo3());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationandMove();
        }
    }
    [PunRPC]
    void Flip(){
        //flipp player
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        //stop the text name to flip
        Vector3 theScale = playerTr.localScale;
        theScale.x *= -1;
        playerTr.localScale = theScale;
        facingRight = !facingRight;
    }
    void UpdateAnimationandMove(){
        if (change != Vector3.zero){
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else{
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter(){
        myRigidbody.MovePosition(transform.position +change.normalized * speed * Time.fixedDeltaTime);
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.1f);
        currentState = PlayerState.walk;
    }
    private IEnumerator AttackCo2()
    {
        animator.SetBool("attacking2", true);
        currentState = PlayerState.attack2;
        yield return null;
        animator.SetBool("attacking2", false);
        yield return new WaitForSeconds(.1f);
        currentState = PlayerState.walk;
    }
    private IEnumerator AttackCo3()
    {
        animator.SetBool("attacking3", true);
        currentState = PlayerState.attack3;
        yield return null;
        animator.SetBool("attacking3", false);
        yield return new WaitForSeconds(.1f);
        currentState = PlayerState.walk;
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }else{
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

}
