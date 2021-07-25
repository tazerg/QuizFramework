using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QuizFramework.Core;
using QuizFramework.Notifications;

namespace QuizFramework.UnitTests
{
    public class NotificationControllerTests
    {
        private const int FirstNotificationSendDelay = 5;
        private const int SecondNotificationSendDelay = 10;

        private const int MinimumTimeInterval = 120;
        private const int MinimumHourOfSend = 9;
        private const int MaximumHourOfSend = 21;

        private INotificationController _notificationController;
        private Mock<ITimeProvider> _mockTimeProvider;

        [SetUp]
        public void OnSetUp()
        {
            var database = new NotificationDatabase(new List<NotificationData>
            {
                new NotificationData(NotificationId.FirstTestNotification, "title", "text",
                    new TimeSpan(0, 0, FirstNotificationSendDelay),
                    null),
                new NotificationData(NotificationId.SecondTestNotifications, "title", "text",
                    new TimeSpan(0, 0, SecondNotificationSendDelay),
                    null)
            });

            var mockConfig = new Mock<INotificationConfig>();
            mockConfig.SetupGet(x => x.MinimumTimeInterval).Returns(MinimumTimeInterval);
            mockConfig.SetupGet(x => x.MinimumHourOfSend).Returns(MinimumHourOfSend);
            mockConfig.SetupGet(x => x.MaximumHourOfSend).Returns(MaximumHourOfSend);

            var sender = new DummyNotificationSender();

            _mockTimeProvider = new Mock<ITimeProvider>();
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(DateTime.Now);

            _notificationController = new NotificationController(mockConfig.Object, database, sender, _mockTimeProvider.Object);
        }

        [TearDown]
        public void OnTearDown()
        {
            _notificationController = null;
            _mockTimeProvider = null;
        }

        [Test]
        public void TestSendNotification()
        {
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestRemoveAllNotification()
        {
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.SecondTestNotifications));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);
            _notificationController.SendNotification(NotificationId.SecondTestNotifications);

            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.SecondTestNotifications));

            _notificationController.RemoveAllNotifications();

            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.SecondTestNotifications));
        }

        [Test]
        public void TestNotificationNotShowed()
        {
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));

            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 15, 0, 0));
            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));

            Task.Run(async () => await WaitSeconds(3)).GetAwaiter().GetResult();

            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestNotificationShowed()
        {
            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));

            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 15, 0, 0));
            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            Assert.AreEqual(true, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));

            Task.Run(async () => await WaitSeconds(6)).GetAwaiter().GetResult();
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 15, 0, 6));

            Assert.AreEqual(false, _notificationController.IsNotificationSent(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestNotificationValidTime1()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 22, 0, 0));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            var expectedTime = new DateTime(2020, 1, 2, 9, 0, 5);
            Assert.AreEqual(expectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestNotificationValidTime2()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 6, 0, 0));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            var expectedTime = new DateTime(2020, 1, 1, 9, 0, 5);
            Assert.AreEqual(expectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestNotificationValidTime3()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 15, 0, 0));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);
            _notificationController.SendNotification(NotificationId.SecondTestNotifications);

            var firstExpectedTime = new DateTime(2020, 1, 1, 15, 0, 5);
            var secondExpectedTime = new DateTime(2020, 1, 1, 15, 2, 5);
            Assert.AreEqual(firstExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
            Assert.AreEqual(secondExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.SecondTestNotifications));
        }

        [Test]
        public void TestNotificationValidTime4()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 15, 0, 0));

            _notificationController.SendNotification(NotificationId.SecondTestNotifications);
            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            var firstExpectedTime = new DateTime(2020, 1, 1, 15, 0, 10);
            var secondExpectedTime = new DateTime(2020, 1, 1, 15, 2, 10);
            Assert.AreEqual(firstExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.SecondTestNotifications));
            Assert.AreEqual(secondExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
        }

        [Test]
        public void TestNotificationValidTime5()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 22, 0, 0));

            _notificationController.SendNotification(NotificationId.FirstTestNotification);
            _notificationController.SendNotification(NotificationId.SecondTestNotifications);

            var firstExpectedTime = new DateTime(2020, 1, 2, 9, 0, 5);
            var secondExpectedTime = new DateTime(2020, 1, 2, 9, 2, 5);
            Assert.AreEqual(firstExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
            Assert.AreEqual(secondExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.SecondTestNotifications));
        }

        [Test]
        public void TestNotificationValidTime6()
        {
            _mockTimeProvider.SetupGet(x => x.LocalTime).Returns(new DateTime(2020, 1, 1, 6, 0, 0));

            _notificationController.SendNotification(NotificationId.SecondTestNotifications);
            _notificationController.SendNotification(NotificationId.FirstTestNotification);

            var firstExpectedTime = new DateTime(2020, 1, 1, 9, 0, 10);
            var secondExpectedTime = new DateTime(2020, 1, 1, 9, 2, 10);
            Assert.AreEqual(firstExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.SecondTestNotifications));
            Assert.AreEqual(secondExpectedTime,
                _notificationController.GetNotificationScheduleTime(NotificationId.FirstTestNotification));
        }

        private async Task WaitSeconds(int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}