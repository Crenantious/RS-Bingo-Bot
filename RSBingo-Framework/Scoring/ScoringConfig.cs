using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework.Scoring;

public record ScoringConfig
{
    public int PointsForEasyTile { get; init; } = 0;
    public int PointsForMediumTile { get; init; } = 0;
    public int PointsForHardTile { get; init; } = 0;
    public int BonusPointsForEasyCompletion { get; init; } = 0;
    public int BonusPointsForMediumCompletion { get; init; } = 0;
    public int BonusPointsForHardCompletion { get; init; } = 0;
    public int BonusPointsForRow { get; init; } = 0;
    public int BonusPointsForColumn { get; init; } = 0;
    public int BonusPointsForDiagonal { get; init; } = 0;
}
