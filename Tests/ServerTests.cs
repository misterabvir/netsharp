using Contracts.Shared;
using Core;
using Core.Implementation;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Net;
using Tests.Fakes;

namespace Tests
{
    public class ServerTests
    {
        private ServerChat _chat;
        private Message _expected;
        private FakeLog _log = new FakeLog();
        private FakeMessageProvider _provider;
        private User _sender = new User() { Name = "Server" };
        ChatContext _dbContext;

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new ChatContext();
            _expected = new Message() { Content = "test", Sender = _sender };
            _provider = new FakeMessageProvider() { Expected = _expected }; ;
            _chat = new ServerChat(
                _provider,
                _dbContext,
                _log);

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
    }
}