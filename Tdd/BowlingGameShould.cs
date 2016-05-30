using System.Collections.Immutable;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Tdd
{
    [TestFixture]
    public class BowlingGameShould
    {
        [SetUp]
        public void SetUp()
        {
            var frames = ImmutableArray.Create(new Frame());
            _game = new Game(frames);
        }

        private Game _game;


        [Test]
        public void HaveEmptyFrame_BeforeAnyRolls()
        {
            var frames = _game.GetFrames();

            Assert.IsNotEmpty(frames);
        }

        [Test]
        public void ThreeStrikesInLine()
        {

            var score = _game.Roll(10).Roll(10).Roll(10).Score;

            Assert.AreEqual(60, score);
        }

        [Test]
        public void Spare()
        {

            var score = _game.Roll(5).Roll(5).Roll(5).Score;

            Assert.AreEqual(20, score);
        }


        [Test]
        public void IncorrectRoll()
        {

            var score = _game.Roll(10).Roll(11).Score;

            Assert.AreEqual(10, score);
        }


        [Test]
        public void AllStrikes()
        {
            for (int i = 0; i < 12; i++)
            {
                _game = _game.Roll(10);
            }

            Assert.AreEqual(300, _game.Score);
        }


        [Test]
        public void ManyLittleRolls()
        {
            for (int i = 0; i < 5; i++)
            {
                _game = _game.Roll(2);
            }

            Assert.AreEqual(10, _game.Score);
        }
    }
}