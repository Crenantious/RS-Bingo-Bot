// <copyright file="BoardFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging;

public class BoardFactory
{
    public Board Create()
    {
        Board board = new();
        board.Image = BoardUpdater.CreateEmptyBoard();
        return board;
    }
}