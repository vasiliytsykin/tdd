using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kontur.Courses.Testing.Tdd
{
    public class Frame
    {
        private int _givenBonusesCount;

        public Frame()
        {
            Hits = new List<int>(2);
        }

        public List<int> Hits { get; }

        public int FrameScore => Hits.Sum() + BonusPoints;

        private int BonusCount => IsStrike ? 2 : IsSpare ? 1 : 0;

        private bool IsStrike => Hits.Count > 0 && Hits.First() == 10;
        private bool IsSpare => Hits.Sum() == 10 && !IsStrike;
        public bool IsDone => Hits.Count > 1 || IsStrike;

        public bool HasBonus => !IsExtraFrame && BonusCount - _givenBonusesCount != 0;

        public bool IsExtraFrame { get; set; }

        public int BonusPoints { get; private set; }

        public bool IsCorrectRoll(int pins) => Hits.Sum() + pins <= 10;

        public void GiveBonus(int pins)
        {
            BonusPoints += pins;
            _givenBonusesCount++;
        }
    }

    public class Game
    {
        private readonly ImmutableArray<Frame> _frames;

        public Game(ImmutableArray<Frame> frames)
        {
            _frames = frames;
        }

        public int Score => _frames.Where(x => !x.IsExtraFrame).Select(x => x.FrameScore).Sum();

        public Game Roll(int pins)
        {
            var frs = _frames.ToList();
            var currentFrame = frs.Last();

            if (currentFrame.IsCorrectRoll(pins))
            {
                foreach (var frame in frs.Where(f => f.HasBonus))
                    frame.GiveBonus(pins);

                currentFrame.Hits.Add(pins);

                if (currentFrame.IsDone)
                    frs.Add(frs.Count < 10 ? new Frame() : new Frame {IsExtraFrame = true});
            }

            return new Game(frs.ToImmutableArray());
        }

        public IList<Frame> GetFrames()
        {
            return _frames.ToList();
        }
    }
}