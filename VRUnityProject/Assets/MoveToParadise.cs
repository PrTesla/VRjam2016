﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveToParadise : MonoBehaviour {

	public Transform character;
	public Transform paradise;

	public float duration = 0f;

	private float    startTime;
	private Vector3 startPosition;
	private bool hastomove = false;
	public AudioSource audioIntro;
	public bool hasLaunchIntro = false;

	public GameObject firstSpot;
	public GameObject secondPost;

	private bool waitToBeRelaunch = false;

	public List<AudioClip> restartAudio;


    // Use this for initialization
    public void StartToMove () 
	{
		startTime = Time.time;
		startPosition = character.position;
		hastomove = true;
	}

    void Start()
    {
        
    }


    void SkipDialogue(Object sender, ClickedEventArgs e)
    {
        StopAllCoroutines();
        StartToMove();
    }

	// Update is called once per frame
	void Update () 
	{
		if (hastomove == false) 
		{
			return;
		}

		float t = (Time.time - startTime) / duration;

		character.transform.position = Vector3.Lerp (startPosition, paradise.position, t);

		if (t >= 1f) 
		{
			hastomove = false;

			SpoonTraquer[] spoonTraquers = GameObject.FindObjectsOfType<SpoonTraquer> ();
			foreach (SpoonTraquer sp in spoonTraquers)
				sp.AddEgg ();

			StartCoroutine (StartGame ());
		}
	}

	public void StartIntro()
	{
		if (hasLaunchIntro == false) 
		{
			hasLaunchIntro = true;
			StartCoroutine (PlayIntro());
		}

		if (waitToBeRelaunch == true) 
		{
			waitToBeRelaunch = false;

			StartCoroutine (RestartGame());
		}
	}

	public void EndGame(bool win)
	{
		waitToBeRelaunch = true;
	}

	IEnumerator PlayIntro ()
	{
		firstSpot.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		audioIntro.Play ();
		yield return new WaitForSeconds (23f);
		secondPost.SetActive (true);
		yield return new WaitForSeconds (1f);
		StartToMove ();
	}

	IEnumerator StartGame ()
	{
		yield return new WaitForSeconds (2f);

		GameObject.FindObjectOfType<ObstacleSpawner> ().StartGame ();
	}


	IEnumerator RestartGame()
	{
		GameObject.FindObjectOfType<GameOver> ().RestartGame ();
        GameObject.FindObjectOfType<ObstacleSpawner>().SetDefaultValues();


        SpoonTraquer[] spoonTraquers = GameObject.FindObjectsOfType<SpoonTraquer> ();
		foreach (SpoonTraquer sp in spoonTraquers)
			sp.Restart ();

		UnityEngine.Assertions.Assert.IsTrue (restartAudio.Count > 0, "No audio clips restart quotes detected im " + name);
		Camera.main.GetComponent<AudioSource>().PlayOneShot(restartAudio[Random.Range(0, restartAudio.Count)]);

		yield return new WaitForSeconds (2f);

		foreach (SpoonTraquer sp in spoonTraquers)
			sp.ReRelease ();

		yield return new WaitForSeconds (1f);

		GameObject.FindObjectOfType<ObstacleSpawner> ().ReStartGame ();
	}
}
