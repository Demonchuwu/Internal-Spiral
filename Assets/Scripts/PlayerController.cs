using CCIJ;
using CCIJ.DialogueSystem;
using CCIJ.Sounds;
using PedroUtils;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator playerAnimator;
    [Header("Audio")]
    [SerializeField] private SFX playerWalkSFX;
    [SerializeField] private SFX playerDrinkSFX;
    [SerializeField] private SFX playerPillSFX;
    [Header("Health")]
    [SerializeField] private float maxHealth = 20.0f;
    [SerializeField] private float health = 20.0f;
    [SerializeField] private float timeToTakeDamage = 10f;
    [Header("Light Intensity")]
    [SerializeField] Light2D myLight;
    [SerializeField] private float lightIntensity = 100;
    [SerializeField] private float maxLightIntensity = 20;
    [SerializeField] private float minLightIntensity = 2f;
    [SerializeField] private float lightIntensityChangeSpeed = 15f;
    [Header("Movement")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    private Rigidbody2D playerRigidbody;
    private Vector2 moveDirection;
    private float targetLightIntensity;
    private bool canMove = true;
    private Image energyBar;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        targetLightIntensity = lightIntensity = maxLightIntensity;

        energyBar = GameObject.Find("Battery").transform.GetChild(1).GetComponent<Image>();

		Time.timeScale = 1;

		InvokeRepeating(nameof(HealthOverTimeLoss), timeToTakeDamage, timeToTakeDamage);
    }

    private void OnEnable()
    {
        var dialogue = FindObjectOfType<DialogueManager>();
        if (dialogue == null) return;

        dialogue.OnDialogueStarted += () => canMove = false;
        dialogue.OnDialogueFinished += OnDialogueFinished;
        dialogue.OnChoiceSelected += IncreaseEnergy;
    }

	private void OnDisable()
    {
        var dialogue = FindObjectOfType<DialogueManager>();
        if (dialogue == null) return;

        dialogue.OnDialogueStarted -= () => canMove = false;
        dialogue.OnDialogueFinished -= OnDialogueFinished;
        dialogue.OnChoiceSelected -= IncreaseEnergy;
    }

    private void OnDialogueFinished(DialogueHolder npc)
    {
		canMove = true;
        if(npc.gameObject.name.Equals("NPC3"))
        {
            Time.timeScale = 0;

            var sceneTransitions = FindObjectOfType<SceneTransitions>();

            if (health <= 0) sceneTransitions.ReloadScene();
            else sceneTransitions.GoToMenu();
		}
	}

    private void IncreaseEnergy(float increase)
    {
        increase *= -1;
        HealthDown(increase, Mathf.RoundToInt(increase));
    }

    private void Update()
    {        
        ProcessInputs();
        Animate();

        UpdateLightIntensity();
        myLight.pointLightOuterRadius = lightIntensity;

        HealthClamp();
        SpeedClamp();
        LightClamp();

        GameOver();
    }

    private void FixedUpdate() => Move();

    public void HealthDown(float damage, int lightReduction) 
    {
        health -= damage;
        targetLightIntensity -= lightReduction;
    }

    public void IncreaseSpeed(float amount) => speed += amount;
    public void IncreaseLightIntensity(float amount) => lightIntensity += amount;

    private void ProcessInputs() 
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = !canMove ? Vector2.zero : new Vector2(moveX, moveY).normalized;
    }

    private void Animate()
	{
        bool isMoving = moveDirection != Vector2.zero;
        if (isMoving)
        {
		    playerAnimator.SetFloat("X", moveDirection.x);
		    playerAnimator.SetFloat("Y", moveDirection.y);
        }
        playerAnimator.Play(isMoving ? "Walk" : "Idle");
	}

    public void PlayWalkSFX() => playerWalkSFX.Play();
    public void PlayDrinkSFX() => playerDrinkSFX.Play();
    public void PlayPillSFX() => playerPillSFX.Play();

    private void UpdateLightIntensity() => lightIntensity = Mathf.Lerp(myLight.pointLightOuterRadius, targetLightIntensity, Time.deltaTime * lightIntensityChangeSpeed);

    private void Move() => playerRigidbody.velocity = moveDirection * speed;

    private void HealthClamp()
    {
        float percentage = health / maxHealth;
        energyBar.fillAmount = percentage;
        ClampValue(ref health, maxHealth);
    }

    private void SpeedClamp() => ClampValue(ref speed, maxSpeed);
    private void LightClamp()
    {
        ClampValue(ref targetLightIntensity, maxLightIntensity, minLightIntensity);
        ClampValue(ref lightIntensity, maxLightIntensity, minLightIntensity);
    }

    private void ClampValue(ref float value, float maxValue, float minValue = 0) => value = Mathf.Clamp(value, minValue, maxValue);

    private void HealthOverTimeLoss()
    {
        if (!canMove) return;
        HealthDown(1.0f, 1);
    }

    private void GameOver()
    {
        if (health > 0 || !canMove || Time.timeScale == 0) return;

        this.Log("GAME OVER");
        FindObjectOfType<SceneTransitions>().ReloadScene();
        Time.timeScale = 0;
    }


}
