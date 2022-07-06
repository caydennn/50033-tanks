using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;

    public bool godMode = false;

    public GameValues gameValues;

    public Slider m_Slider;

    public Image m_FillImage;

    public HealthBar healthBar;

    public Color m_FullHealthColor = Color.green;

    public Color m_ZeroHealthColor = Color.red;

    public GameObject m_ExplosionPrefab;

    private AudioSource m_ExplosionAudio;

    private ParticleSystem m_ExplosionParticles;

    private float m_CurrentHealth;

    private bool m_Dead;

    public TankManager tankManager;

    private void Awake()
    {
        m_ExplosionParticles =
            Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);

        // find object with tag HealthBar
        // check if current object is tag player
        // if so, set healthbar to this object
        // if not, set healthbar to null
        if (gameObject.tag == "Player")
        {
            healthBar =
                GameObject
                    .FindGameObjectWithTag("HealthBar")
                    .GetComponent<HealthBar>();

            healthBar.SetMaxHealth((int) m_StartingHealth);
        }
        else
        {
            healthBar = null;
        }
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount, GameObject shooter)
    {
        // if this is the player and godmode is on, do nothing
        if (gameObject.tag == "Player" && godMode)
        {
            return;
        }

        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;

        SetHealthUI();

        Debug.Log("shooter: " + shooter.name);
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            if (shooter.tag == "Player" && gameObject.tag == "Enemy")
            {
                // only increase the score if the shooter is the player
                gameValues.playerScore += 1;
                if (gameValues.playerScore > gameValues.highScore)
                {
                    gameValues.highScore = gameValues.playerScore;
                }
            }

            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        if (healthBar != null)
        {
            Debug.Log("Player health; " + m_CurrentHealth);
            healthBar.SetHealth((int) m_CurrentHealth);
        }
        else
        {
            // healthBar.GetComponent<HealthBar>().SetHealth((int) m_CurrentHealth);
            m_Slider.value = m_CurrentHealth;
            m_FillImage.color =
                Color
                    .Lerp(m_ZeroHealthColor,
                    m_FullHealthColor,
                    m_CurrentHealth / m_StartingHealth);
        }
    }

    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        if (gameObject.tag == "Player")
        {
            HandlePlayerDeath();
        }
        else
        {
            // Enemies can respawn indefinitely
            // wait for a few seconds
            // then respawn
            Invoke("_Respawn", gameValues.enemyRespawnTime);
            gameObject.SetActive(false);
        }

        // if the player died
    }

    private void HandlePlayerDeath()
    {
        // need to decrement player lives
        // if player lives is 0, game over
        // if player lives is > 0, respawn player
        gameValues.playerLives -= 1;
        if (gameValues.playerLives <= 0)
        {
            gameObject.SetActive(false);

            gameValues.gameOver = true;
        }
        else
        {
            _Respawn();
        }
    }

    private void _Respawn()
    {
        Debug.Log("Respawning player");
        tankManager.Reset();
    }

    // private void Respawn()
    // {
    //     m_Instance.transform.position = m_SpawnPoint.position;
    //     m_Instance.transform.rotation = m_SpawnPoint.rotation;

    //     m_Instance.SetActive(false);
    //     m_Instance.SetActive(true);
    // }
}
