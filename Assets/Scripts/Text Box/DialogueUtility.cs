using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using TMPro;

public class DialogueUtility : MonoBehaviour
{
    // grab the remainder of the text until ">" or end of string
    private const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";
    private const string PAUSE_REGEX_STRING = "<p:(?<pause>" + REMAINDER_REGEX + ")>";
    private static readonly Regex pauseRegex = new Regex(PAUSE_REGEX_STRING);
    private const string SPEED_REGEX_STRING = "<sp:(?<speed>" + REMAINDER_REGEX + ")>";
    private static readonly Regex speedRegex = new Regex(SPEED_REGEX_STRING);
    private const string ANIM_START_REGEX_STRING = "<anim:(?<anim>" + REMAINDER_REGEX + ")>";
    private static readonly Regex animStartRegex = new Regex(ANIM_START_REGEX_STRING);
    private const string ANIM_END_REGEX_STRING = "</anim>";
    private static readonly Regex animEndRegex = new Regex(ANIM_END_REGEX_STRING);
    private const string ALIGN_REGEX_STRING = "<align:(?<align>" + REMAINDER_REGEX + ")>";
    private static readonly Regex alignRegex = new Regex(ALIGN_REGEX_STRING);
    private const string NAME_REGEX_STRING = "<name:(?<name>" + REMAINDER_REGEX + ")>";
    private static readonly Regex nameRegex = new Regex(NAME_REGEX_STRING);
    private const string SPEAKER_REGEX_STRING = "<face:(?<face>" + REMAINDER_REGEX + ")>";
    private static readonly Regex speakerRegex = new Regex(SPEAKER_REGEX_STRING);
    private const string SPEAK_DIR_REGEX_STRING = "<facing:(?<dir>" + REMAINDER_REGEX + ")>";
    private static readonly Regex speakDirRegex = new Regex(SPEAK_DIR_REGEX_STRING);

    private static readonly Dictionary<string, float> pauseDictionary = new Dictionary<string, float>{
        { "tiny", .1f },
        { "short", .25f },
        { "normal", 0.666f },
        { "long", 1f },
        { "read", 2f },
    };

    public static List<DialogueCommand> ProcessInputString(string message, out string processedMessage)
    {
        List<DialogueCommand> result = new List<DialogueCommand>();
        processedMessage = message;

        processedMessage = HandlePauseTags(processedMessage, result);
        processedMessage = HandleSpeedTags(processedMessage, result);
        processedMessage = HandleAnimStartTags(processedMessage, result);
        processedMessage = HandleAnimEndTags(processedMessage, result);
        processedMessage = HandleNameTags(processedMessage, result);
        processedMessage = HandleAlignTags(processedMessage, result);
        processedMessage = HandleSpeakerTags(processedMessage, result);
        processedMessage = HandleFacingTags(processedMessage, result);

        return result;
    }
    
    private static string HandleFacingTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection nameMatches = speakDirRegex.Matches(processedMessage);
        foreach (Match match in nameMatches)
        {
            string stringVal = match.Groups["dir"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.SpeakerFace,
                stringValue = stringVal
            });
        }
        processedMessage = Regex.Replace(processedMessage, SPEAK_DIR_REGEX_STRING, "");
        return processedMessage;
    }
    
    private static string HandleSpeakerTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection nameMatches = speakerRegex.Matches(processedMessage);
        foreach (Match match in nameMatches)
        {
            string stringVal = match.Groups["speaker"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Speaker,
                stringValue = stringVal
            });
        }
        processedMessage = Regex.Replace(processedMessage, SPEAKER_REGEX_STRING, "");
        return processedMessage;
    }
    
    private static string HandleNameTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection nameMatches = nameRegex.Matches(processedMessage);
        foreach (Match match in nameMatches)
        {
            string stringVal = match.Groups["name"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Name,
                stringValue = stringVal
            });
        }
        processedMessage = Regex.Replace(processedMessage, NAME_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleAlignTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection centerStartMatches = alignRegex.Matches(processedMessage);
        foreach (Match match in centerStartMatches)
        {
            string stringVal = match.Groups["align"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Align,
                textAlignOptions = GetTextAlignType(stringVal)
            });
        }
        processedMessage = Regex.Replace(processedMessage, ALIGN_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleAnimEndTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection animEndMatches = animEndRegex.Matches(processedMessage);
        foreach (Match match in animEndMatches)
        {
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.AnimEnd,
            });
        }
        processedMessage = Regex.Replace(processedMessage, ANIM_END_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleAnimStartTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection animStartMatches = animStartRegex.Matches(processedMessage);
        foreach (Match match in animStartMatches)
        {
            string stringVal = match.Groups["anim"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.AnimStart,
                textAnimValue = GetTextAnimationType(stringVal)
            });
        }
        processedMessage = Regex.Replace(processedMessage, ANIM_START_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleSpeedTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection speedMatches = speedRegex.Matches(processedMessage);
        foreach (Match match in speedMatches)
        {
            string stringVal = match.Groups["speed"].Value;
            if (!float.TryParse(stringVal, out float val))
            {
                val = 150f;
            }
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.TextSpeedChange,
                floatValue = val
            });
        }
        processedMessage = Regex.Replace(processedMessage, SPEED_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandlePauseTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection pauseMatches = pauseRegex.Matches(processedMessage);
        foreach (Match match in pauseMatches)
        {
            string val = match.Groups["pause"].Value;
            string pauseName = val;
            Debug.Assert(pauseDictionary.ContainsKey(pauseName), "no pause registered for '" + pauseName + "'");
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Pause,
                floatValue = pauseDictionary[pauseName]
            });
        }
        processedMessage = Regex.Replace(processedMessage, PAUSE_REGEX_STRING, "");
        return processedMessage;
    }
    
    private static TextAlignOptions GetTextAlignType(string stringVal)
    {
        TextAlignOptions result;
        try
        {
            result = (TextAlignOptions)Enum.Parse(typeof(TextAlignOptions), stringVal, true);
        }
        catch (ArgumentException)
        {
            Debug.LogError("Invalid Text Alignment Type: " + stringVal);
            result = TextAlignOptions.left;
        }
        return result;
    }

    private static TextAnimationType GetTextAnimationType(string stringVal)
    {
        TextAnimationType result;
        try
        {
            result = (TextAnimationType)Enum.Parse(typeof(TextAnimationType), stringVal, true);
        }
        catch (ArgumentException)
        {
            Debug.LogError("Invalid Text Animation Type: " + stringVal);
            result = TextAnimationType.none;
        }
        return result;
    }

    private static int VisibleCharactersUpToIndex(string message, int index)
    {
        int result = 0;
        bool insideBrackets = false;
        for (int i = 0; i < index; i++)
        {
            if (message[i] == '<')
            {
                insideBrackets = true;
            }
            else if (message[i] == '>')
            {
                insideBrackets = false;
                result--;
            }
            if (!insideBrackets)
            {
                result++;
            }
            else if (i + 6 < index && message.Substring(i, 6) == "sprite")
            {
                result++;
            }
        }
        return result;
    }
}
public struct DialogueCommand
{
    public int position;
    public DialogueCommandType type;
    public float floatValue;
    public string stringValue;
    public TextAnimationType textAnimValue;
    public TextAlignOptions textAlignOptions;
}

public enum DialogueCommandType
{
    Pause,
    TextSpeedChange,
    AnimStart,
    AnimEnd,
    Align,
    Name,
    Speaker,
    SpeakerFace
}

public enum TextAlignOptions
{
    left,
    right,
    topCenter,
    midCenter
}

public enum TextAnimationType
{
    none,
    shake,
    wave
}
