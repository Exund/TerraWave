using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using FMOD;
using FMODUnity;

namespace Exund.TerraWave
{
	class MusicLoader : MonoBehaviour
	{
		int index = -1;
		public static List<Sound> sounds = new List<Sound>();
		FMOD.System system;
		ChannelGroup master;
		Sound current;
		Channel channel;
		bool init = false;
		uint position;
		uint last_position;
		uint length;
		bool playing = false;

		void Start()
		{
			var sounds_path = Path.Combine(TerraWaveMod.assets_path, "Sounds");
			system = RuntimeManager.LowlevelSystem;
			system.getMasterChannelGroup(out master);
			foreach(string file in Directory.EnumerateFiles(sounds_path, "*.wav").Union(Directory.EnumerateFiles(sounds_path, "*.mp3")))
			{
				Console.WriteLine(file);
				sounds.Add(LoadSound(file));
			}
			if(sounds.Count == 0)
			{
				GameObject.DestroyImmediate(this);
				return;
			}
			Singleton.Manager<ManGameMode>.inst.ModeStartEvent.Subscribe(OnModeStart);
			Singleton.Manager<ManGameMode>.inst.ModeCleanUpEvent.Subscribe(OnModeCleanUp);
			useGUILayout = false;
		}

		Sound LoadSound(string path)
		{
			Sound result;
			system.createSound(path, MODE.CREATESAMPLE | MODE.ACCURATETIME, out result);
			return result;
		}

		private void OnModeStart(Mode mode)
		{
			if(mode.GetGameType() == ManGameMode.GameType.RaD) index = 0;
		}

		private void OnModeCleanUp(Mode mode)
		{
			if(init)
			{
				channel.stop();
				useGUILayout = init = false;
			}
		}

		void Update()
		{
			if(!playing && index != -1)
			{
				current = sounds[index];
				current.getLength(out length, TIMEUNIT.MS);
				system.playSound(current, master, false, out channel);
				playing = true;
				init = true;
			}
			if(init && playing)
			{
				last_position = position;
				channel.getPosition(out position, TIMEUNIT.MS);
				
				bool paused;
				channel.getPaused(out paused);
				if(position == 0 && (paused || last_position + 100 > length))
				{
					Stop();
					index++;
					if (index >= sounds.Count) index -= sounds.Count;
				}
			}
			if (Input.GetKeyDown(KeyCode.K)) useGUILayout = !useGUILayout;
		}

		void OnGUI()
		{
			
			if(init) GUI.Window(7790, new Rect(0, 0, 400, 100), DoWindow, "ＴｅｒｒａＷａｖｅ");
		}

		private void DoWindow(int id)
		{
			current.getName(out string name, 32);
			GUILayout.Label(name);
			GUILayout.HorizontalSlider(position, 0, length);
			GUILayout.BeginHorizontal();
			bool prev = GUILayout.Button("<<");
			bool next = GUILayout.Button(">>");
			GUILayout.EndVertical();

			if (prev || next) Stop();
			if (prev) index--;
			if (next) index++;

			if (index < 0) index = sounds.Count + index;
			if (index >= sounds.Count) index -= sounds.Count;
		}

		void Stop()
		{
			if (init)
			{
				channel.stop();
				playing = false;
			}
		}
	}
}
