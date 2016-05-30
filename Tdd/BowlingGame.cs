using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Kontur.Courses.Testing.Tdd
{
    public class Frame
    {
        private readonly List<int> _hits = new List<int>();
        private int givenBonusesCount;

        private int bonusCount => IsStrike ? 2 : IsSpare ? 1 : 0;

        private bool IsStrike => _hits.Count > 0 && _hits.First() == 10;
        private bool IsSpare => _hits.Sum() == 10 && !IsStrike;
        public bool IsDone => _hits.Count > 1 || IsStrike;

        public bool HasBonus => bonusCount - givenBonusesCount != 0;

        public int BonusPoints { get; private set; }

        public void GiveBonus(int pins)
        {
            BonusPoints += pins;
            givenBonusesCount++;
        }

        public void AddHit(int pins)
        {
            if (_hits.Sum() + pins <= 10)
                _hits.Add(pins);
            else throw new ArgumentException();
        }
    }

    public class Game
    {
        private readonly ImmutableArray<Frame> _frames;
        private int _score;

        public Game(ImmutableArray<Frame> frames, int score = 0)
        {
            _frames = frames;
            _score = score;
        }

        public int Score => _score + TotalBonus;


        private int TotalBonus => _frames.Select(x => x.BonusPoints).Sum();

        public Game Roll(int pins)
        {
            var frs = _frames.ToList();
            var currentFrame = frs.Last();

            foreach (var frame in frs.Where(f => f.HasBonus))
                frame.GiveBonus(pins);

            _score += pins;

            try
            {
                currentFrame.AddHit(pins);
                if (currentFrame.IsDone)
                    frs.Add(new Frame());
            }
            catch (ArgumentException)
            {
            }

            return new Game(frs.ToImmutableArray(), _score);
        }

        public IList<Frame> GetFrames()
        {
            return _frames.ToList();
        }
    }
}