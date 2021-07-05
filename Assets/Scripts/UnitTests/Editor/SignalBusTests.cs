using NUnit.Framework;
using QuizFramework.SignalBus;

namespace QuizFramework.UnitTests
{
    public class SignalBusTests
    {
        private ISignalBus _signalBus;

        private int _testValue;

        [SetUp]
        public void OnSetup()
        {
            _signalBus = new SignalBus.SignalBus();
        }

        [TearDown]
        public void OnTearDown()
        {
            _testValue = 0;
            
            _signalBus = null;
        }

        [Test]
        public void SignalTest()
        {
            _signalBus.Subscribe<TestSignal>(OnTestSignal);
            
            Assert.AreEqual(0, _testValue);
            
            _signalBus.Fire(new TestSignal());
            
            Assert.AreEqual(1, _testValue);
        }

        [Test]
        public void SignalTestWithUnsubscribe()
        {
            _signalBus.Subscribe<TestSignal>(OnTestSignal);
            
            Assert.AreEqual(0, _testValue);
            
            _signalBus.Fire(new TestSignal());
            
            Assert.AreEqual(1, _testValue);
            
            _signalBus.Unsubscribe<TestSignal>(OnTestSignal);
            _signalBus.Fire(new TestSignal());
            
            Assert.AreEqual(1, _testValue);
        }

        [Test]
        public void GenericSignalTest()
        {
            _signalBus.Subscribe<TestSignal>(OnTestSignalGeneric);
            
            Assert.AreEqual(0, _testValue);
            
            _signalBus.Fire(new TestSignal {TestValue = 3});
            
            Assert.AreEqual(3, _testValue);
        }
        
        [Test]
        public void ManyGenericSignalTest()
        {
            _signalBus.Subscribe<TestSignal>(OnTestSignalGeneric);
            
            Assert.AreEqual(0, _testValue);
            
            _signalBus.Fire(new TestSignal {TestValue = 3});
            
            Assert.AreEqual(3, _testValue);
            
            _signalBus.Fire(new TestSignal {TestValue = 5});
            
            Assert.AreEqual(8, _testValue);
        }
        
        [Test]
        public void GenericSignalTestWithUnsubscribe()
        {
            _signalBus.Subscribe<TestSignal>(OnTestSignalGeneric);
            
            Assert.AreEqual(0, _testValue);
            
            _signalBus.Fire(new TestSignal {TestValue = 3});
            
            Assert.AreEqual(3, _testValue);
            
            _signalBus.Unsubscribe<TestSignal>(OnTestSignalGeneric);
            _signalBus.Fire(new TestSignal {TestValue = 3});
            
            Assert.AreEqual(3, _testValue);
        }

        private void OnTestSignal()
        {
            _testValue += 1;
        }

        private void OnTestSignalGeneric(TestSignal signal)
        {
            _testValue += signal.TestValue;
        }

        private struct TestSignal
        {
            public int TestValue;
        }
    }
}