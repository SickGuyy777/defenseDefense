using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Scripts.Path_System;
using UnityEngine;
using UnityEngine.Assertions;

public static class PathInterpreter
{
    /// <summary>
    /// The string representation of the Token.Start token.
    /// </summary>
    public const string StartToken = "start";
    /// <summary>
    /// The string representation of the Token.Line token.
    /// </summary>
    public const string LineToken = "line";
    /// <summary>
    /// The string representation of the Token.End token.
    /// </summary>
    public const string EndToken = "end";

    public static IEnumerable<Vector2> PositionsFromFile(PathFile pathFile)
    {
        // Get all the lines from the pathFile's data
        var lines = pathFile.data.Split('\n');
        
        // Assertions
        Assert.AreNotEqual(0, lines.Length);
        
        var positions = new List<Vector2>();

        // Get positions
        foreach (var line in lines)
        {
            if (line.Length == 0) // Blank line
                continue;;

            // Check if we've reached an end token
            if (line.ToLower() == EndToken)
                break;
            
            // The token ends in the first ' ' (space)
            var tokenAsString = line[..line.IndexOf(' ')];

            var token = GetToken(tokenAsString);

            // Undefined check
            Assert.IsFalse(token == Token.Undefined);

            // Either Start or Line
            var positionAsString = line.Split(' ');
            var position = GetPosition(positionAsString[1], positionAsString[2]);
            
            positions.Add(position);
        }
        
        static Vector2 GetPosition(string xString, string yString)
        {
            if (!float.TryParse(xString, out var x) || !float.TryParse(yString, out var y))
                throw new ArgumentException("x or y positions invalid.");

            return new Vector2(x, y);
        }

        return positions;
    }
    
    /// <summary>
    /// Get a <see cref="Path"/> object from lines in a .path file.
    /// </summary>
    /// <param name="pathFile">A <see cref="PathFile"/> scriptable object which contains the path data.</param>
    /// <returns></returns>
    public static Path FromFile(PathFile pathFile)
    {
        // Get positions
        var positions = PositionsFromFile(pathFile);
        
        // Create and return the path
        return Path.New(positions, "Path");
    }
    
    /// <summary>
    /// Get a value in the <see cref="Token"/> enum from a string.
    /// </summary>
    /// <param name="token">The token as a string</param>
    /// <returns>
    ///     The value which corresponds to <paramref name="token"/> in the <see cref="Token"/> enum.
    ///     Returns <see cref="Token.Undefined"/> if <paramref name="token"/> doesn't correspond to any enum value.
    /// </returns>
    public static Token GetToken(string token)
    {
        // Get the token in lower case
        token = token.ToLower();

        return token switch
        {
            StartToken => Token.Start,
            LineToken => Token.Line,
            EndToken => Token.End,
            _ => Token.Undefined
        };
    }
    
    /// <summary>
    /// Every <see cref="Token"/> defines a command in a .path file.
    /// </summary>
    public enum Token
    {
        Undefined, Start, Line, End
    }
}
