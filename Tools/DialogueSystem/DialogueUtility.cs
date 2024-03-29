﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace SimpleTools.DialogueSystem {
	public class DialogueUtility : MonoBehaviour {

		// grab the remainder of the text until ">" or end of string
		const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";
		const string PAUSE_REGEX_STRING = "<p:(?<pause>" + REMAINDER_REGEX + ")>";
		static readonly Regex pauseRegex = new Regex(PAUSE_REGEX_STRING);
		const string SOUND_REGEX_STRING = "<snd:(?<sound>" + REMAINDER_REGEX + ")>";
		static readonly Regex soundRegex = new Regex(SOUND_REGEX_STRING);
		const string PLAYMUSIC_REGEX_STRING = "<playmsc:(?<playmusic>" + REMAINDER_REGEX + ")>";
		static readonly Regex playMusicRegex = new Regex(PLAYMUSIC_REGEX_STRING);
		const string STOPMUSIC_REGEX_STRING = "<stopmsc:(?<stopmusic>" + REMAINDER_REGEX + ")>";
		static readonly Regex stopMusicRegex = new Regex(STOPMUSIC_REGEX_STRING);
		const string SPEED_REGEX_STRING = "<sp:(?<speed>" + REMAINDER_REGEX + ")>";
		static readonly Regex speedRegex = new Regex(SPEED_REGEX_STRING);
		const string ANIM_START_REGEX_STRING = "<anim:(?<anim>" + REMAINDER_REGEX + ")>";
		static readonly Regex animStartRegex = new Regex(ANIM_START_REGEX_STRING);
		const string ANIM_END_REGEX_STRING = "</anim>";
		static readonly Regex animEndRegex = new Regex(ANIM_END_REGEX_STRING);

		static readonly Dictionary<string, float> pauseDictionary = new Dictionary<string, float>{
		{ "tiny", .1f },
		{ "short", .25f },
		{ "normal", 0.666f },
		{ "long", 1f },
		{ "read", 2f },
	};

		public static List<DialogueCommand> ProcessInputString(string message, out string processedMessage) {
			List<DialogueCommand> result = new List<DialogueCommand>();
			processedMessage = message;

			processedMessage = HandlePauseTags(processedMessage, result);
			processedMessage = HandleSoundTags(processedMessage, result);
			processedMessage = HandlePlayMusicTags(processedMessage, result);
			processedMessage = HandleStopMusicTags(processedMessage, result);
			processedMessage = HandleSpeedTags(processedMessage, result);
			processedMessage = HandleAnimStartTags(processedMessage, result);
			processedMessage = HandleAnimEndTags(processedMessage, result);

			return result;
		}

		static string HandleAnimEndTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection animEndMatches = animEndRegex.Matches(processedMessage);
			foreach (Match match in animEndMatches) {
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.AnimEnd,
				});
			}
			processedMessage = Regex.Replace(processedMessage, ANIM_END_REGEX_STRING, "");
			return processedMessage;
		}

		static string HandleAnimStartTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection animStartMatches = animStartRegex.Matches(processedMessage);
			foreach (Match match in animStartMatches) {
				string stringVal = match.Groups["anim"].Value;
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.AnimStart,
					textAnimValue = GetTextAnimationType(stringVal)
				});
			}
			processedMessage = Regex.Replace(processedMessage, ANIM_START_REGEX_STRING, "");
			return processedMessage;
		}

		static string HandleSpeedTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection speedMatches = speedRegex.Matches(processedMessage);
			foreach (Match match in speedMatches) {
				string stringVal = match.Groups["speed"].Value;
				if (!float.TryParse(stringVal, out float val)) {
					val = 150f;
				}
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.TextSpeedChange,
					floatValue = val
				});
			}
			processedMessage = Regex.Replace(processedMessage, SPEED_REGEX_STRING, "");
			return processedMessage;
		}

		static string HandlePauseTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection pauseMatches = pauseRegex.Matches(processedMessage);
			foreach (Match match in pauseMatches) {
				string val = match.Groups["pause"].Value;
				string pauseName = val;
				Debug.Assert(pauseDictionary.ContainsKey(pauseName), "no pause registered for '" + pauseName + "'");
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.Pause,
					floatValue = pauseDictionary[pauseName]
				});
			}
			processedMessage = Regex.Replace(processedMessage, PAUSE_REGEX_STRING, "");
			return processedMessage;
		}
		static string HandleSoundTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection soundMatches = soundRegex.Matches(processedMessage);
			foreach (Match match in soundMatches) {
				string val = match.Groups["sound"].Value;
				string soundName = val;
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.Sound,
					stringValue = soundName
				});
			}
			processedMessage = Regex.Replace(processedMessage, SOUND_REGEX_STRING, "");
			return processedMessage;
		}
		static string HandlePlayMusicTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection playMatches = playMusicRegex.Matches(processedMessage);
			foreach (Match match in playMatches) {
				string val = match.Groups["playmusic"].Value;
				string functionName = val;
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.PlayMusic,
					stringValue = functionName
				});
			}
			processedMessage = Regex.Replace(processedMessage, PLAYMUSIC_REGEX_STRING, "");
			return processedMessage;
		}
		static string HandleStopMusicTags(string processedMessage, List<DialogueCommand> result) {
			MatchCollection stopMatches = stopMusicRegex.Matches(processedMessage);
			foreach (Match match in stopMatches) {
				string val = match.Groups["stopmusic"].Value;
				string functionName = val;
				result.Add(new DialogueCommand {
					position = VisibleCharactersUpToIndex(processedMessage, match.Index),
					type = DialogueCommandType.StopMusic,
					stringValue = functionName
				});
			}
			processedMessage = Regex.Replace(processedMessage, STOPMUSIC_REGEX_STRING, "");
			return processedMessage;
		}

		static TextAnimationType GetTextAnimationType(string stringVal) {
			TextAnimationType result;
			try {
				result = (TextAnimationType)Enum.Parse(typeof(TextAnimationType), stringVal, true);
			} catch (ArgumentException) {
				Debug.LogError("Invalid Text Animation Type: " + stringVal);
				result = TextAnimationType.none;
			}
			return result;
		}

		static int VisibleCharactersUpToIndex(string message, int index) {
			int result = 0;
			bool insideBrackets = false;
			for (int i = 0; i < index; i++) {
				if (message[i] == '<') {
					insideBrackets = true;
				} else if (message[i] == '>') {
					insideBrackets = false;
					result--;
				}
				if (!insideBrackets) {
					result++;
				} else if (i + 6 < index && message.Substring(i, 6) == "sprite") {
					result++;
				}
			}
			return result;
		}
	}
	public struct DialogueCommand {
		public int position;
		public DialogueCommandType type;
		public float floatValue;
		public string stringValue;
		public TextAnimationType textAnimValue;
	}

	public enum DialogueCommandType {
		Pause,
		TextSpeedChange,
		AnimStart,
		AnimEnd,
		Sound,
		PlayMusic,
		StopMusic
	}

	public enum TextAnimationType {
		none,
		shake,
		wave,
		wobble,
		rainbow,
	}
}