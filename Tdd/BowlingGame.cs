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

        private int BonusCount => IsStrike ? 2 : IsSpare ? 1 : 0;
        private bool IsStrike => Hits.Count > 0 && Hits.First() == 10;
        private bool IsSpare => Hits.Sum() == 10 && !IsStrike;

        public List<int> Hits { get; }
        public int FrameScore => Hits.Sum() + BonusPoints;
        public bool IsDone => Hits.Count > 1 || IsStrike;
        public bool HasBonus => !IsExtraFrame && BonusCount - _givenBonusesCount != 0;
        public bool IsExtraFrame { get; set; }
        public int BonusPoints { get; private set; }

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
            if (IsCorrectRoll(pins))
            {
                var updatedFrames = UpdateFrames(GiveBonuses(_frames.ToList(), pins), pins);
                return new Game(updatedFrames.ToImmutableArray());
            }

            return this;
        }

        private List<Frame> GiveBonuses(List<Frame> frames, int pins)
        {
            foreach (var frame in frames.Where(f => f.HasBonus))
                frame.GiveBonus(pins);

            return frames;
        }

        private List<Frame> UpdateFrames(List<Frame> frames, int pins)
        {
            var currentFrame = frames.Last();
            currentFrame.Hits.Add(pins);

            if (currentFrame.IsDone)
                frames.Add(frames.Count < 10 ? new Frame() : new Frame {IsExtraFrame = true});

            return frames;
        }

        private bool IsCorrectRoll(int pins) =>      
             _frames.Last().Hits.Sum() + pins <= 10;
        

        public IList<Frame> GetFrames()
        {
            return _frames.ToList();
        }
    }
}