using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	static public AudioManager instance;

	public List<AudioClip> playerNotes;
	public List<AudioClip> allNotes;
	public int numberOfAudioSources = 5;
	private AudioSource[] audioSources;
	private int beatCnt;

	private readonly int[,] chordC = {
		{1,1,7+1,2,7+3,3,7+5,4,4,0},
		{1,1,7+3,3,7+5,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
		{1,1,7+3,3,7+5,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
		{1,1,7+3,3,7+5,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
	};
	private readonly int[,] chordF = {
		{4,1,7+1,2,7+4,3,7+6,4,4,0},
		{4,1,7+4,3,7+6,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
		{4,1,7+4,3,7+6,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
		{4,1,7+4,3,7+6,4,8,0,0,0},
		{7+1,2,8,0,0,0,0,0,0,0},
	};
	private readonly int[,] chordDm = {
		{2,1,7+2,2,7+4,3,7+6,4,4,0},
		{2,1,7+4,3,7+6,4,8,0,0,0},
		{7+2,2,8,0,0,0,0,0,0,0},
		{2,1,7+4,3,7+6,4,8,0,0,0},
		{7+2,2,8,0,0,0,0,0,0,0},
		{2,1,7+4,3,7+6,4,8,0,0,0},
		{7+2,2,8,0,0,0,0,0,0,0},
	};

	struct NotesPerPlay
	{
		public List<KeyValuePair<int,int>> notes;
		public int type;
		public NotesPerPlay(int[] row)
		{
			notes = new List<KeyValuePair<int, int>>();
			type = 0;
			for (int i = 0; i < row.Length; i += 2)
			{
				if (row[i + 1] == 0)
				{
					type = row[i];
					break;
				}
				notes.Add(new KeyValuePair<int, int>(row[i] - 1, row[i + 1]));
			}
		}
	}
	
	struct Chord
	{
		public List<NotesPerPlay> notesPerPlay;
		public Chord(int[,] rawChord)
		{
			notesPerPlay = new List<NotesPerPlay>();
			for(int i = 0; i < rawChord.GetLength(0); ++i)
			{
				int[] row = new int[rawChord.GetLength(1)];
				for (int j = 0; j < rawChord.GetLength(1); ++j)
					row[j] = rawChord[i, j];
				notesPerPlay.Add(new NotesPerPlay(row));
			}
		}
	}

	private Chord[] chords;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);

		audioSources = new AudioSource[numberOfAudioSources];

		chords = new Chord[3];
		chords[0] = new Chord(chordC);
		chords[1] = new Chord(chordF);
		chords[2] = new Chord(chordDm);

		for(int i = 0; i < numberOfAudioSources; ++i)
		{
			audioSources[i] = gameObject.AddComponent<AudioSource>();
			audioSources[i].loop = false;
			audioSources[i].playOnAwake = false;
			audioSources[i].volume = 0.3f;
		}
		audioSources[0].volume = 0.6f;

		beatCnt = 4;

		GameManager.instance.Beat += OnBeat;
	}
	
	private void OnBeat(object sender, EventArgs eventArgs)
	{
		beatCnt = beatCnt % 4 + 1;
		if(beatCnt == 1)
		{
			if (Enemy.instance == null) StartCoroutine(PlayChord(chords[0]));
			else StartCoroutine(PlayChord(chords[(int)Enemy.instance.state]));
		}
	}

	IEnumerator PlayChord(Chord chord)
	{
		foreach(NotesPerPlay notesPerPlay in chord.notesPerPlay)
		{
			foreach(KeyValuePair<int, int> notes in notesPerPlay.notes)
				PlayClipAtSpecificSource(allNotes[notes.Key],audioSources[notes.Value]);

			float factor = 0;
			switch (notesPerPlay.type)
			{
				case 0:yield break;
				case 4:factor = 1f;break;
				case 8:factor = 0.5f;break;
				case 16:factor = 0.25f;break;
			}

			yield return new WaitForSeconds(GameManager.instance.secondsPerBeat * factor);
		}
	}

	private void PlayClipAtSpecificSource(AudioClip clip,AudioSource audioSource)
	{
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void PlayNote(int index)
	{
		PlayClipAtSpecificSource(playerNotes[index], audioSources[0]);
	}

	private void OnDestroy()
	{
		GameManager.instance.Beat -= OnBeat;
	}
}
