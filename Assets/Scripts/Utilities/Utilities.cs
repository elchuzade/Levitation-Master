using System.Collections.Generic;
using UnityEngine;
using System;

public class Utilities : MonoBehaviour
{
    // Hide this
    static string secret = "mostsecretkeyever";

    private static string AddDateToString(string date, List<char> textList) {
        // 1 time 1 char ... until full timestring is built
        if (textList.Count < 13)
        {
            Debug.Log("String is too small to add time string to it");
            return "";
        }
        List<char> combinedText = new List<char>();
        for (int i = 0; i < date.Length; i++)
        {
            combinedText.Add(date[i]);
            combinedText.Add(textList[i]);
        }
        List<char> shortTextList = new List<char>();
        // Take values of the textList after 13 since we have first 13 values inside combinedText
        for (int i = 13; i < textList.Count; i++)
        {
            shortTextList.Add(textList[i]);
        }

        for (int i = 0; i < shortTextList.Count; i++)
        {
            combinedText.Add(shortTextList[i]);
        }
        string result = "";
        for (int i = 0; i < combinedText.Count; i++)
        {
            result += combinedText[i];
        };
        return result;
    }

    private static List<int> DuplicateListToMatch (List<int> list, List<int> match)
    {
        List<int> duplicatedList = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            duplicatedList.Add(list[i]);
        }
        while (duplicatedList.Count < match.Count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                duplicatedList.Add(list[i]);
            }
        }
        return duplicatedList;
    }

    private static List<int> AddNoiseToList (List<int> list, List<int> noise) {
        if (noise.Count < list.Count)
        {
            Debug.Log("Error. Noise must be equal or bigger than list");
            throw new Exception();
        }
        List<int> noiseList = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            noiseList.Add(list[i] + noise[i]);
        }
        return noiseList;
    }

    private static List<int> RemoveNoiseToList(List<int> list, List<int> noise)
    {
        if (noise.Count < list.Count)
        {
            Debug.Log("Error. Noise must be equal or bigger than list");
            throw new Exception();
        }
        List<int> noiseList = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            noiseList.Add(list[i] - noise[i]);
        }
        return noiseList;
    }

    public static string Encoder(string message)
    {
        /* STEP 1: TURN CURRENT DATE INTO SINGLE DIGITS */
        DateTimeOffset now = DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        string fullDate = date.ToString();
        string dateString = fullDate.Substring(fullDate.Length - 4, 4);
        // Turn date string into array of chars of each digit
        char[] dateArray = dateString.ToCharArray();
        // Turn array of chars into list of integers
        List<int> dateAscii = new List<int>();
        for (int i = 0; i < dateArray.Length; i++) { dateAscii.Add(int.Parse(dateArray[i].ToString())); }
        Console.WriteLine(dateAscii);

        // Turn secret string into array of chars
        char[] secretArray = secret.ToCharArray();
        // Turn array of chars into list of integers of their ascii codes
        List<int> secretAsciiGrouped = new List<int>();
        for (int i = 0; i < secretArray.Length; i++) { secretAsciiGrouped.Add(secretArray[i]); }
        // Combine all ascii codes into a single string
        string secretString = "";
        for (int i = 0; i < secretAsciiGrouped.Count; i++) { secretString += secretAsciiGrouped[i]; }
        // Turn combined ascii codes string into a array of chars that represent each digit
        char[] secretAsciiArray = secretString.ToCharArray();
        // Turn array of single digit integers into list of integers
        List<int> secretAscii = new List<int>();
        for (int i = 0; i < secretAsciiArray.Length; i++) { secretAscii.Add(int.Parse(secretAsciiArray[i].ToString())); }
        Console.WriteLine(secretAscii);

        /* STEP 3: TURN MESSAGE INTO ASCII */
        // Turn message string into array of chars
        char[] messageArray = message.ToCharArray();
        // Turn array of chars into list of integers of their ascii codes
        List<int> messageAscii = new List<int>();
        for (int i = 0; i < messageArray.Length; i++) { messageAscii.Add(messageArray[i]); }

        /* STEP 4: ADD DATE ASCII TO SECRET ASCII */
        List<int> duplicatedDateSecret = DuplicateListToMatch(dateAscii, secretAscii);
        // console.log('duplicatedDate: ', duplicatedDateSecret)

        List<int> dateSecretAscii = AddNoiseToList(secretAscii, duplicatedDateSecret);
        // console.log('dateSecretAscii: ', dateSecretAscii)

        /* STEP 5: ADD DATE ASCII TO MESSAGE ASCII */
        List<int> duplicatedDateMessage = DuplicateListToMatch(dateAscii, messageAscii);
        // console.log('duplicatedDateMessage: ', duplicatedDateMessage)
        List<int> dateMessageAscii = AddNoiseToList(messageAscii, duplicatedDateMessage);
        // console.log('dateMessageAscii: ', dateMessageAscii)

        /* STEP 6: ADD DATE-SECRET ASCII TO DATE-MESSAGE ASCII */
        List<int> duplicateSecretMessage = DuplicateListToMatch(dateSecretAscii, dateMessageAscii);
        // console.log('duplicateSecretMessage: ', duplicateSecretMessage)
        List<int> dateSecretDateMessageAscii = AddNoiseToList(dateMessageAscii, duplicateSecretMessage);
        // console.log('dateSecretDateMessageAscii: ', dateSecretDateMessageAscii)

        /* STEP 7: TURN MESSAGE ASCII TO STRING */
        List<char> textMessage = new List<char>();
        for (int i = 0; i < dateSecretDateMessageAscii.Count; i++) { textMessage.Add((char)dateSecretDateMessageAscii[i]); };
        // console.log('textMessage: ', textMessage)

        /* STEP 8: ADD DATE TO MESSAGE STRING */
        string result = AddDateToString(fullDate, textMessage);
        return result;
    }

    public static string BuildHeaders(string message)
    {
        string encoded = Encoder(message);
        string encodedEscape = Uri.EscapeUriString(encoded);
        encodedEscape = encodedEscape.Replace("(", "&#40");
        encodedEscape = encodedEscape.Replace(")", "&#41");
        return encodedEscape;
    }
}
