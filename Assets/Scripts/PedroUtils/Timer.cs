// maded by Pedro M Marangon
using System;
using UnityEngine;

namespace PedroUtils
{
	public class Timer
	{

		//-------------PRIVATE VARIABLES---------------//
		private string _name;
		private bool _shouldDebug;
		private float _startingSeconds;

		//-------------CONSTRUCTORS---------------//

		/// <summary>
		/// Creates a new timer that can be debugged
		/// </summary>
		/// <param name="name">A name for this timer (for easier debugging)</param>
		/// <param name="seconds">The amount of seconds the timer will count down</param>
		/// <param name="shouldDebug">Should this timer shown in the Console when it started and when it ended?</param>
		public Timer(string name, float seconds, bool shouldDebug = true)
		{
			_name = name;
			_shouldDebug = shouldDebug;
			_startingSeconds = seconds;
			RemainingSeconds = seconds;
		}
		/// <summary>
		/// Creates a new timer
		/// </summary>
		/// <param name="seconds">The amount of seconds the timer will count down</param>
		public Timer(float seconds) => new Timer("", seconds, false);
		/// <summary>
		/// Creates a new timer, already setting what will happen once the timer ends
		/// </summary>
		/// <param name="name">A name for this timer (for easier debugging)</param>
		/// <param name="seconds">The amount of seconds the timer will count down</param>
		/// <param name="timerEnd">What will happen once the timer ends</param>
		/// <param name="startTicking">Should the timer start ticking immediately?</param>
		public Timer(string name, float seconds, Action timerEnd, bool startTicking = true, bool shouldDebug = true)
		{
			_name = name;
			_shouldDebug = shouldDebug;
			_startingSeconds = RemainingSeconds = seconds;
			OnTimerEnd = timerEnd;

			if (startTicking) StartTicking();
		}

		//-------------PUBLIC PROPERTIES---------------//

		public bool IsTimerRunning { get; private set; }
		/// <summary>
		/// How many seconds there are left
		/// </summary>
		public float RemainingSeconds { get; private set; }
		/// <summary>
		/// How finished this timer is
		/// </summary>
		public float Percentage => RemainingSeconds / _startingSeconds;

		//-------------EVENTS---------------//

		/// <summary>
		/// Event to decide what will happen when the timer ends
		/// </summary>
		public event Action OnTimerEnd;
		/// <summary>
		/// Event that will run on each tick of the timer, with the current remaining seconds
		/// </summary>
		public event Action<float> OnTimerTick;
		/// <summary>
		/// What will happen when the timer suddenly change time
		/// </summary>
		public event Action OnTimerChange;

		//-------------PUBLIC METHODS---------------//

		/// <summary>
		/// Start ticking with the initial second count
		/// </summary>
		public void StartTicking() => StartTicking(_startingSeconds);

		/// <summary>
		/// Start ticking with a custom value for the second count
		/// </summary>
		/// <param name="seconds"></param>
		public void StartTicking(float seconds)
		{
			if(RemainingSeconds <= 0) AddSeconds(seconds);
			if (_shouldDebug) Debug.Log($"{_name}: Start ticking... {seconds}s");
			IsTimerRunning = true;
		}
		
		public void StopTicking()
		{
			RemainingSeconds = 0;
			if (_shouldDebug) Debug.Log($"{_name}: Stop ticking...");
			IsTimerRunning = false;
		}

		public void AddSeconds(float secondsToAdd)
		{
			RemainingSeconds += secondsToAdd;
			OnTimerChange?.Invoke();
		}

		public void RemoveSeconds(float secondsToRemove)
		{
			RemainingSeconds -= secondsToRemove;
			OnTimerChange?.Invoke();
		}

		/// <summary>
		/// Tick the timer, by subtracting the remaining seconds
		/// </summary>
		/// <param name="deltaTime">How much should RemainingSeconds be subtracted by? (For FPS-independent value, use Time.deltaTime)</param>
		public void Tick(float deltaTime)
		{
			if (RemainingSeconds <= 0f || !IsTimerRunning) return;
			RemainingSeconds -= deltaTime;
			OnTimerTick?.Invoke(RemainingSeconds);

			CheckForTimerEnd();
		}

		/// <summary>
		/// Tick the timer using Time.deltaTime
		/// </summary>
		public void Tick() => Tick(Time.deltaTime);

		//-------------PRIVATE METHODS---------------//

		private void CheckForTimerEnd()
		{
			if (RemainingSeconds > 0f) return;

			RemainingSeconds = 0;

			if (_shouldDebug) Debug.Log($"{_name}: TimerFinished");
			
			IsTimerRunning = false;

			OnTimerTick?.Invoke(RemainingSeconds);
			OnTimerEnd?.Invoke();
		}
	}
}
