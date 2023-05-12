// <copyright file="CompiledMessagesTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using static RSBingoBot.MessageUtilities;

[TestClass]
public class CompiledMessagesTests : MockDBBaseTestClass
{
    private const int MaxChars = 10;
    private const int NewLineChars = 2;
    private const int MaxCharsNoLine = MaxChars - NewLineChars;
    private const int MaxCharsNoLineMinusOne = MaxChars - NewLineChars - 1;

    [DataTestMethod]
    [DataRow(new int[] { 0 }, new int[] { 0 })]
    [DataRow(new int[] { 1 }, new int[] { 1 })]
    [DataRow(new int[] { 1, 1, 1 }, new int[] { 3 + NewLineChars * 2 })]
    [DataRow(new int[] { 1, 1, 1, MaxChars, 1, 1, 1 }, new int[] { 3 + NewLineChars *2, MaxChars, 3 + NewLineChars *2 })]
    [DataRow(new int[] { MaxChars }, new int[] { MaxChars })]
    [DataRow(new int[] { MaxChars, 1 }, new int[] { MaxChars, 1 })]
    [DataRow(new int[] { MaxChars + 1 }, new int[] { MaxChars, 1 })]
    [DataRow(new int[] { MaxChars * 3 }, new int[] { MaxChars, MaxChars, MaxChars })]
    [DataRow(new int[] { MaxCharsNoLine, 1 }, new int[] { MaxCharsNoLine, 1 })]
    [DataRow(new int[] { MaxCharsNoLineMinusOne, 1 }, new int[] { MaxChars })]
    [DataRow(new int[] { MaxCharsNoLineMinusOne, 2 }, new int[] { MaxCharsNoLineMinusOne, 2 })]
    public void CreateNonCSVFile_Parse_GetException(int[] inputMessagesChars, int[] outputMessagesChars)
    {
        IEnumerable<string> inputMessages = inputMessagesChars.Select(i => new string('c', i));

        IEnumerable<string> compiledMessages = GetCompiledMessages(inputMessages, MaxChars);

        Assert.AreEqual(outputMessagesChars.Length, compiledMessages.Count());

        for (int i = 0; i < outputMessagesChars.Length; i++)
        {
            Assert.AreEqual(outputMessagesChars[i], compiledMessages.ElementAt(i).Count());
        }
    }
}