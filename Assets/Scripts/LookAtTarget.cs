using UnityEngine;
using System.Collections;
using TMPro;




public class LookAtTarget : MonoBehaviour {

    [Tooltip("This is the object that the script's game object will look at by default")]
    public GameObject defaultTarget; // the default target that the camera should look at

    [Tooltip("This is the object that the script's game object is currently look at based on the player clicking on a gameObject")]
    public GameObject currentTarget; // the target that the camera should look at

    public string dropdownText;
    public GameObject dropdownTarget;

    //GameObject previousTarget; this is the previous target for switching Audio - no longer required with StopAllAudio

    // Start happens once at the beginning of playing. This is a great place to setup the behavior for this gameObject
    void Start () {
		if (defaultTarget == null) 
		{
            defaultTarget = this.gameObject;
			Debug.Log ("defaultTarget target not specified. Defaulting to parent GameObject");
		}

        if (currentTarget == null)
        {
            currentTarget = defaultTarget; 
            Debug.Log("currentTarget target not specified. Defaulting to defaultTarget GameObject");
        }

        //previousTarget = defaultTarget; // initially there is no previous target - no longer required with StopAllAudio

    }

	
	// Update is called once per frame
    // For clarity, Update happens constantly as your game is running
    void Update()
    {

        // if primary mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // determine the ray from the camera to the mousePosition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // cast a ray to see if it hits any gameObjects
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            // if there are hits
            if (hits.Length>0)
            {
                // get the first object hit
                RaycastHit hit = hits[0];
                currentTarget = hit.collider.gameObject;
                StopAllAudio();
                AudioSource currentAudio = currentTarget.GetComponent<AudioSource>();
                currentAudio.Play();
                Debug.Log("currentTarget changed to "+currentTarget.name);
                dropdown.value = dropdown.options.FindIndex(option => option.text == currentTarget.name);
            }
        } else if (Input.GetMouseButtonDown(1)) // if the second mouse button is pressed
        {
            StopAllAudio();
            AudioSource default_audio = defaultTarget.GetComponent<AudioSource>();
            default_audio.Play();
            currentTarget = defaultTarget;
            Debug.Log("currentTarget changed to " + currentTarget.name);
            dropdown.value = dropdown.options.FindIndex(option => option.text == currentTarget.name);
        }

       // if a currentTarget is set, then look at it
        if (currentTarget!=null)
        {
            // transform here refers to the attached gameobject this script is on.
            // the LookAt function makes a transform point it's Z axis towards another point in space
            // In this case it is pointing towards the target.transform
            transform.LookAt(currentTarget.transform);
            
        } else // reset the look at back to the default
        {
            currentTarget = defaultTarget;
            Debug.Log("defaultTarget changed to " + currentTarget.name);
        }
    }

    [SerializeField] private TMP_Dropdown dropdown;

    public void getDropdownValue()
    {
        int selectIndex = dropdown.value;
        dropdownText = dropdown.options[selectIndex].text;
        Debug.Log("Dropdown changed to " + dropdownText);
        if (dropdownText != null)
        {
            dropdownTarget = GameObject.Find(dropdownText);
            StopAllAudio();
            AudioSource target_audio = dropdownTarget.GetComponent<AudioSource>();
            target_audio.Play();
            currentTarget = dropdownTarget;
        }
    }

    AudioSource[] allAudioSources;
    // Code to stop all audio before playing a new audio
    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}


