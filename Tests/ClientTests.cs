using Contracts.Shared;
using Core;
using Core.Implementation;
using System.Net;
using Tests.Fakes;

namespace Tests
{
    public class ClientTests
    {
        private ClientChat _chat;
        private FakeLog _log = new FakeLog();
        private FakeUserInput _input = new FakeUserInput();
        private FakeMessageProvider _provider;
        private Message _expected;
        private User _sender = new User() { Name = "username" };
       
        [SetUp]
        public async Task Setup()
        {
            _expected = new Message() { Content = "test", Sender = _sender };
            _provider = new FakeMessageProvider() { Expected = _expected };
            _chat = new ClientChat(
                _sender.Name,
                _provider,
                new IPEndPoint(IPAddress.Loopback, 0),
                _log,
                _input);

            await _chat.StartAsync();
        }

        [Test]
        public void TestListenMessage()
        {           
            Message actual = _log.Actual;
            Assert.Multiple(() =>
            {
                Assert.That(actual.Content, Is.EqualTo(_expected.Content));
                Assert.That(actual.Time, Is.EqualTo(_expected.Time));
                Assert.That(actual.Sender, Is.EqualTo(_expected.Sender));
            });
        }

        [Test]
        public async Task TestSendingMessage()
        {
            Message actual = _provider.Actual;
            Assert.That(actual.CommandType, Is.EqualTo(Command.Join));
            Assert.That(_sender.Name, Is.EqualTo(actual.Sender.Name));

            _input.Expected = "test";
            _input.SendFlag = true;
            await Task.Delay(100);
            actual = _provider.Actual;
            Assert.That(actual.CommandType, Is.EqualTo(Command.None));
            Assert.That(_sender.Name, Is.EqualTo(actual.Sender.Name));


            _input.Expected = "/users";
            _input.SendFlag = true;
            await Task.Delay(100);
            actual = _provider.Actual;
            Assert.That(actual.CommandType, Is.EqualTo(Command.Users));
            Assert.That(_sender.Name, Is.EqualTo(actual.Sender.Name));


            _input.Expected = "/exit";
            _input.SendFlag = true;
            await Task.Delay(100);
            actual = _provider.Actual;
            Assert.That(actual.CommandType, Is.EqualTo(Command.Leave));
            Assert.That(_sender.Name, Is.EqualTo(actual.Sender.Name));
        }
    }
}