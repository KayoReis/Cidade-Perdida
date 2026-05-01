
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

  CharacterController controller;

  public Transform dad;

  Vector3 X, Y, Z;

  float xspeed = 10f;
  float zspeed = 10f;

  float gravity;
  float speedjump;
  float maxJumpHeigth = 2f;
  float timeToMaxHeigth = 0.5f;

  float originalspeedx, originalspeedz, crouchX = 5, CrouchZ = 5, RunX = 15, RunZ = 15;

  float Crouchtime = 0;

  [SerializeField] Image StaminaBar;

  [SerializeField] GameObject BarStamina;

private float staminaMax = 3.5f;
  public float stamina = 3.5f;

  public float delayregen = 1f;
  float delaytimer = 0;

  bool iscrouching, isRunning;
  public bool isEmpty;

  public Camera cam;

  public int health = 100;

  private float offsetY;
  
  Coroutine bleedingCouroutine;

[SerializeField] private GameObject blood;

[SerializeField] private Death death;

private float minFallSpeed = 10f, maxFallSpeed = 50f, verticalVelocity;

private int maxDamage=100;

private bool isGrounded, wasGrounded;
  void Start()
  {
    blood.SetActive(false);
    BarStamina.SetActive(false);
    controller = GetComponent<CharacterController>();
    gravity = (-2 * maxJumpHeigth) / (timeToMaxHeigth * timeToMaxHeigth);
    speedjump = (2 * maxJumpHeigth) / timeToMaxHeigth;
    originalspeedx = xspeed;
    originalspeedz = zspeed;
    dad.localPosition = new Vector3(-110.968f, 0, 103.684f);
    transform.localPosition = new Vector3(-0.03199768f, 1f, 0.05296326f);
    controller.height = 2f;
    controller.center = new Vector3(0, 0, 0);
    offsetY = dad.transform.position.y - transform.position.y;

  }
  void Update()
  {
    dad.transform.position=new Vector3(transform.position.x,transform.position.y+offsetY,transform.position.z);
    //mover
    float XInput = Input.GetAxisRaw("Vertical");
    float ZInput = Input.GetAxisRaw("Horizontal");

    X = XInput * xspeed * transform.forward;
    Z = ZInput * zspeed * transform.right;
    Y += gravity * Time.deltaTime * Vector3.up;

    //Pular
    if (controller.isGrounded)
    {
      Y = Vector3.down;
    }

    if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded && !iscrouching)
    {
      Y = speedjump * Vector3.up;
    }

    if (Y.y > 0 && (controller.collisionFlags & CollisionFlags.Above) != 0)
    {
      Y = Vector3.zero;
    }

    //velocidade final
    Vector3 finalVelocity = X + Y + Z;

    controller.Move(finalVelocity * Time.deltaTime);

    // Agachar
    if (Input.GetKey(KeyCode.LeftControl) && !isRunning && controller.isGrounded)
    {
      controller.height = 1f;
      controller.center = new Vector3(0, 0.5f, 0);
      dad.transform.localScale = new Vector3(1, 0.5f, 1);
      iscrouching = true;
      xspeed = crouchX;
      zspeed = CrouchZ;

    }
    if (iscrouching)
    {
      Crouchtime += Time.deltaTime;
    }
    if ((Input.GetKeyUp(KeyCode.LeftControl) && Crouchtime > 0.3f) || (Crouchtime > 0.3f && !Input.GetKey(KeyCode.LeftControl)))
    {
      
      controller.height = 2f;
      controller.center = new Vector3(0, 0, 0);
      dad.transform.localScale = new Vector3(1, 1, 1);
      xspeed = originalspeedx;
      zspeed = originalspeedz;
      iscrouching = false;
      Crouchtime = 0;
    }

    //correr
    if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && delaytimer <= 0)
    {
      BarStamina.SetActive(true);
      stamina -= Time.deltaTime;
      xspeed = RunX;
      zspeed = RunZ;
      isRunning = true;

      if (stamina <= 0)
      {
        stamina = 0;
        isEmpty = true;
        delaytimer = delayregen;

      }

    }
    if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && stamina <= 0))
    {
      
      xspeed = originalspeedx;
      zspeed = originalspeedz;
      isRunning = false;
     
    }
    if (!isRunning && stamina < 3.5f)
    {
      if (isEmpty)
      {
       
        delaytimer -= Time.deltaTime;
        xspeed = 3f;
        zspeed = 3f;
        cam.TremorAtivo = true;

        if (delaytimer <= 0)
        {
          xspeed = originalspeedx;
          zspeed = originalspeedz;
          isEmpty = false;
        }
      }
      else
      {
        cam.TremorAtivo = false;
        stamina += 0.5f * Time.deltaTime;
        if (stamina >= staminaMax)
        {
          BarStamina.SetActive(false);
          stamina = 3.5f;
        }
      }
    }

    if(BarStamina.activeSelf)
    {
      StaminaBar.fillAmount = stamina / staminaMax;
    }
    if (health <= 0)
    {
      xspeed = 0;   
      zspeed = 0;

      death.Dead();
    }

    isGrounded = controller.isGrounded;

    if (!isGrounded)
    {
      verticalVelocity = controller.velocity.y;
    }

    if(!wasGrounded && isGrounded)
    {
      float fallSpeed = -verticalVelocity;

      if( fallSpeed > minFallSpeed)
      {
        StartCoroutine(Tremor());
        Bleeding();
        float t = Mathf.InverseLerp(minFallSpeed, maxFallSpeed, fallSpeed);
        int damage = Mathf.RoundToInt(t*maxDamage);

        health -= damage;
      }
    }

  }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
    {
      Debug.Log("touch");
      health -= 20;
      Blooded();
    }
    }
    void Blooded()
  {
    if (bleedingCouroutine != null)
    {
      StopCoroutine(bleedingCouroutine);
    }

   bleedingCouroutine = StartCoroutine(Bleeding());
  }

  IEnumerator Bleeding()
  {
    blood.SetActive(true);
    yield return new WaitForSeconds(1f);
    blood.SetActive(false);

    bleedingCouroutine = null;
    
  }

  IEnumerator Tremor()
  {
    cam.TremorAtivo = true;
    yield return new WaitForSeconds(0.8f);
    cam.TremorAtivo = false;
  }


}

