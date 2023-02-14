using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework.Models;

public partial class Team : IHasScore
{
    private TeamScore? teamScore;

    public int Score => TeamScore.Score;

    public TeamScore TeamScore => teamScore ??= new();

    public void UpdateScore(Tile tile)
    {
        TeamScore.Update(tile);
    }
}
