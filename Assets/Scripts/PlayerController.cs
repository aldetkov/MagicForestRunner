using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public delegate void SwitchMove();
    public event SwitchMove OnSwitchMove;
    Animator animator;
    CharacterController cc;
    public UIEvents gameController;
    public SoundsController soundsController;

    public float distance = 3f;
    public float heightJump = 3f;
    public float gravity = 5f;
    public float distanceToClimb = 3f;
    public float heightClimb = 5f;
    public GameObject gameOverFlag;

    float time;
    float timeJump;
    float timeClimb = 0f;
    float currentDistance = 0f;
    float roadMoveBan;
    float moveBack = 2f;
    public float score = 0f;

    bool corutinActive = false;
    bool isJump = false;
    bool climb = false;
    bool isZoneClimb = false;
    public bool death = false;
    bool isRun = false;
    bool isJumpParticle = false;

    public ParticleSystem gameOverParticle;
    public ParticleSystem hitDeathParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem runParticle;

    Rigidbody[] allGOwithRB;
    
    private void Awake()
    {
        if (PlayerController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerController.instance = this;
        allGOwithRB = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        soundsController = GameObject.FindGameObjectWithTag("soundsController").GetComponent<SoundsController>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        // Присвоение времени анимаций
        time = 0.53f;
        timeJump = 1f / 1.2f;
        timeClimb = 2.7f;
    }


    // Update is called once per frame
    void Update()
    {
        if (!death && !corutinActive)
        { 
            // Переход на соседнюю дорожку
            float dir = Input.GetAxisRaw("Horizontal");
            if (dir != 0) StartCoroutine(Move(dir));    

            // Прыжок + карабканье
            if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) StartCoroutine(Jump());

            // Падение (проверка на отсутствие впереди персонажа поверхности, отсутствие позади персонажа поверхности и работы корутин, 
            // чтобы движение не пересекалось с другими)
            if (!Physics.Raycast(transform.position + Vector3.up + (Vector3.back / 3), -transform.up.normalized, 3f)
                && !Physics.Raycast(transform.position + Vector3.up + (Vector3.forward / 3), -transform.up.normalized, 3f))
            {
                StartCoroutine(Fall());
            }

            // Карабканье
            if (climb && isZoneClimb)
            {
                StartCoroutine(Climb());
            }

            // Включение партикла движения
            if (!corutinActive && !isRun) RunParticlePlayAndStop();
        }
        // Гравитация

        if (!isJump && !climb) Gravity();

        // Выключение партикла движения
        if (corutinActive && isRun) RunParticlePlayAndStop();

        if (Input.GetKeyDown(KeyCode.Escape)) gameController.PauseGame();


    }

    private void OnTriggerEnter(Collider other)
    {
        // Столкновение с препятствием
        if (other.CompareTag("Danger") && !death)
        {
            hitDeathParticle.Play();
            soundsController.PlaySound(SoundsController.HIT);
            animator.SetTrigger("Death");
            death = true;
            isJump = false;
            climb = false;
            if (WorldController.instance.isMovement == true) OnOrOffMove();
            StartCoroutine(MoveBack());
            gameController.Death();
        }
        
        // Столкновение со стеной для карабканья
        if (other.CompareTag("Climb"))
        {
            heightClimb = (other.transform.lossyScale.y - transform.position.y);
            climb = true;
            isJump = false;
        }

        // Прыжок с нужного места для карабканья
        if (other.CompareTag("CanClimb"))
        {
            heightClimb = (other.transform.lossyScale.y - transform.position.y);
            isZoneClimb = true;
        }

        // Подбор монет
        if (other.CompareTag("Coin")) {
            soundsController.PlaySound(SoundsController.TAKECOIN);
            Destroy(other.gameObject);
            score += 1;
        }

    }


    /// <summary>
    /// Смещение на соседнюю дорожку
    /// </summary>
    /// <param name="dir">Направление движения, 1 - направо, -1 - налево</param>
    IEnumerator Move(float dir)
    {
        // Проверка на наличие соседней дорожки и отсутствия препятсвия справа
        if (dir > 0 && roadMoveBan < 1 && !Physics.Raycast(transform.position + Vector3.up / 5f, transform.right, distance))
        {
            corutinActive = true;
            animator.SetTrigger("Right");
        }
        // Проверка на наличие соседней дорожки и отсутствия препятсвия слева
        if (dir < 0 && roadMoveBan > -1 && !Physics.Raycast(transform.position + Vector3.up / 5f, -transform.right, distance))
        {
            corutinActive = true;
            animator.SetTrigger("Left");
        }

        // Движение
        if (corutinActive) 
        {
            roadMoveBan += dir;
            currentDistance = distance;
            soundsController.PlaySound(SoundsController.SOMERSAULT);
            while (currentDistance > 0)
            {
                float speed = distance / time;
                float tmpDist = Time.fixedDeltaTime * speed;
                Vector3 direction = Vector3.right * dir * tmpDist;
                cc.Move(direction);
                currentDistance -= tmpDist;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.03f);
        }
        corutinActive = false;
    }

    /// <summary>
    /// Прыжок + проверка на карабканье
    /// </summary>
    IEnumerator Jump()
    {
        corutinActive = true;
        animator.SetTrigger("Jump");
        currentDistance = heightJump;
        isJump = true;
        yield return new WaitForFixedUpdate();
        soundsController.PlaySound(SoundsController.JUMP);
        while (isJump)
        {
            if (animator.GetFloat("JumpCurve") > 0)
            {
                if (!isJumpParticle)
                {
                    isJumpParticle = true;
                    jumpParticle.gameObject.transform.position = transform.position;
                    jumpParticle.Play();
                }
                Up(heightJump, timeJump);
            }

            if (animator.GetFloat("JumpCurve") < 0 || climb)
            {
                isJump = false;
                isJumpParticle = false;
            }
            yield return new WaitForFixedUpdate();
        }
        corutinActive = false;

    }

    /// <summary>
    /// Карабканье
    /// </summary>
    IEnumerator Climb()
    {
        corutinActive = true;
        isJump = false;
        if (WorldController.instance.isMovement == true) OnOrOffMove();
        currentDistance = heightClimb + 0.2f;
        animator.SetTrigger("Climb");
        while (currentDistance > 0)
        {
            Up(heightClimb, timeClimb);
            if (!Physics.Raycast(transform.position, transform.forward, 2f)) climb = false;
            yield return new WaitForFixedUpdate();
        }
        if (WorldController.instance.isMovement == false) OnOrOffMove();
        yield return new WaitForSeconds(0.03f);
        climb = false;
        isZoneClimb = false;
        corutinActive = false;
    }

    /// <summary>
    /// Падение
    /// </summary>
    IEnumerator Fall()
    {
        corutinActive = true;
        animator.SetTrigger("Fall");
        WorldController.instance.speed /= 2f;
        
        while (true)
        {
            if (Physics.Raycast(transform.position, -transform.up, 0.3f))
            {
                animator.SetTrigger("FallDown");
                soundsController.PlaySound(SoundsController.SOMERSAULT);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.8f);

        WorldController.instance.speed *= 2f;
        corutinActive = false;
    }

    /// <summary>
    /// Движение назад при проигрыше
    /// </summary>
    IEnumerator MoveBack()
    {
        corutinActive = true;
        if (Random.Range(0, 5)  < 1)
        {

        currentDistance = moveBack;
        while (currentDistance > 0)
        { 
            float speed = moveBack * 2f;
            float tmpDist = Time.deltaTime * speed;
            Vector3 direction = Vector3.back * tmpDist;
        
            cc.Move(direction);
        
            currentDistance -= tmpDist;
        
            yield return new WaitForEndOfFrame();
        }
        gameOverParticle.Play();

        } else
        {
            EnableRagdoll();
            animator.enabled = false;
        }
        CreateGameOverFlag();
        yield return new WaitForSeconds(0.03f);
        corutinActive = false;
    }

    /// <summary>
    /// Поднятие персонажа вверх
    /// </summary>
    /// <param name="distance">Высота</param>
    /// <param name="time">Время на поднятие</param>
    void Up(float distance, float time)
    {
        float speed = distance / time;
        float tmpDist = Time.fixedDeltaTime * speed;
        Vector3 direction = Vector3.up * tmpDist;
        cc.Move(direction);
        currentDistance -= tmpDist;
    }

    /// <summary>
    /// Гравитация
    /// </summary>
    void Gravity()
    {
        cc.Move(Vector3.down * gravity * Time.deltaTime);
    }

    /// <summary>
    /// Включение и выключение движение мира
    /// </summary>
    void OnOrOffMove()
    {
        if (OnSwitchMove != null) OnSwitchMove();
    }
 
    /// <summary>
    /// Включает и выключает частицы бега
    /// </summary>
    void RunParticlePlayAndStop()
    {

        if (isRun)
        {
            isRun = false;
            runParticle.Stop();
        }
        else
        {
            isRun = true;
            runParticle.Play();
        }

    }

    /// <summary>
    /// Увеличение скорости анимаций
    /// </summary>
    /// <param name="koefSpeedAdd"></param>
    public void SetSpeedAnimation(float koefSpeedAdd)
    {
        animator.speed *= koefSpeedAdd;
        time /= koefSpeedAdd;
        timeJump /= koefSpeedAdd;
        timeClimb /= koefSpeedAdd;
    }

    void CreateGameOverFlag()
    {
        Vector3 pos = transform.position + Vector3.up * 10f;
        Quaternion angle = default;
        if (roadMoveBan == -1)
        {
            pos += Vector3.right;
            angle = Quaternion.Euler(gameOverFlag.transform.rotation.eulerAngles + Vector3.up * 45f);
        }
        else
        {
            pos += Vector3.left;
            angle = Quaternion.Euler(gameOverFlag.transform.rotation.eulerAngles + Vector3.down * 45f);
        }

        Instantiate(gameOverFlag,  pos, angle, transform); 
    }
    void EnableRagdoll ()
    {
        foreach (var i in allGOwithRB)
        {

            i.isKinematic = false;
        }
    }
    void DisableRagdoll()
    {
        foreach (var i in allGOwithRB)
        {
            i.isKinematic = true;
        }
    }
}