using RSBingo_Framework.Models;
using RSBingo_Framework.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework.Interfaces;

public interface IHasScore
{
    public int Score { get; }

    public TeamScore TeamScore { get; }

    public void UpdateScore(Tile tile);
}
