using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 1000;
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] private float levelLoadDelay = 2f;
    private bool collisionsDisabled = false;
    
    Rigidbody _rb;
    AudioSource _audioSource;

    enum State 
    { 
        Alive, 
        Dying, 
        Transcending 
    }
    State state = State.Alive;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextSceen();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friend":

                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                //kill
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke(nameof(LoadFirstLevel), levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke(nameof(LoadNextSceen), levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void LoadNextSceen()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextSceneToLoad = currentScene + 1;
        if (nextSceneToLoad == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneToLoad = 0;
        }
        SceneManager.LoadScene(nextSceneToLoad);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            _audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rb.AddRelativeForce(Vector3.up * (rcsThrust * Time.deltaTime));
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
    private void RespondToRotateInput()
    {
        _rb.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * (rotationSpeed * Time.deltaTime));
        }

    }
}
