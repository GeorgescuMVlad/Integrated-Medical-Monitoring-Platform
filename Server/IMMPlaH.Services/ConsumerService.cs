using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMMPlaH.DataAccess;
using IMMPlaH.Domain.Models;
using IMMPlaH.Services.Abstractions;
using IMMPlaH.Services.Exceptions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace IMMPlaH.Services
{
    public class ConsumerService : BackgroundService, IConsumerService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;
        private readonly AppDbContext _dbContext;

        List<Activities> activities = new List<Activities>();
        List<ActivitiesProblems> activitiesProblems = new List<ActivitiesProblems>();

        public ConsumerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _hostname = "jaguar.rmq.cloudamqp.com";
            _port = 1883;
            _queueName = "activitiesQueue";
            _username = "iuddgiof:iuddgiof";
            _password = "UejeQCfS_x15mY0Kc7DkSwIOBZZx153D";
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                Port = _port,
                UserName = _username,
                Password = _password
            };

            factory.Uri = new System.Uri("amqps://iuddgiof:UejeQCfS_x15mY0Kc7DkSwIOBZZx153D@jaguar.rmq.cloudamqp.com/iuddgiof");
            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.Span);
                Activities activity = new Activities();
                activity = System.Text.Json.JsonSerializer.Deserialize<Activities>(content);
                activities.Add(activity);

                if(activity.End - activity.Start > 25200000 && activity.Activity.Equals("Sleeping"))
                {
                    DateTime startConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.Start);
                    DateTime endConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.End);

                    ActivitiesProblems activityProblem = new ActivitiesProblems();
                    activityProblem.PatientId = activity.PatientId;
                    activityProblem.Activity = activity.Activity;
                    activityProblem.Start = startConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activityProblem.End = endConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activitiesProblems.Add(activityProblem);
                }

                if (activity.End - activity.Start > 18000000 && activity.Activity.Equals("Leaving"))
                {
                    DateTime startConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.Start);
                    DateTime endConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.End);

                    ActivitiesProblems activityProblem = new ActivitiesProblems();
                    activityProblem.PatientId = activity.PatientId;
                    activityProblem.Activity = activity.Activity;
                    activityProblem.Start = startConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activityProblem.End = endConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activitiesProblems.Add(activityProblem);
                }

                if (activity.End - activity.Start >= 1800000 && activity.Activity.Equals("Toileting"))
                {
                    DateTime startConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.Start);
                    DateTime endConverted = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(activity.End);

                    ActivitiesProblems activityProblem = new ActivitiesProblems();
                    activityProblem.PatientId = activity.PatientId;
                    activityProblem.Activity = activity.Activity;
                    activityProblem.Start = startConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activityProblem.End = endConverted.ToString("yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                    activitiesProblems.Add(activityProblem);
                }

                /* Am facut aici comparatia cu 248 pentru ca stiam ca sunt 248 de obiecte trimise in queue conform
                 fisierului activities.txt si am facut aceasta comparatie ca sa pot adauga toata lista o data in 
                baza de date cu BulkInsert. E mult mai eficient asa si practic nu fac 248 de calls la db adica call
                de save pt fiecare obiect. Nu am facut insertul cu lista dupa linia 124 pentru ca nu mai sunt vizibile
                elementele listei in afara zonei de consume, asa functioneaza.*/
                if (activities.Count == 248)
                {
                    saveActivity(activities, activitiesProblems);
                    activities.Clear();
                    activitiesProblems.Clear();
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        public void saveActivity(List<Activities> activities, List<ActivitiesProblems> activitiesProblems)
        {
            try
            {
                _dbContext.BulkInsert(activities);
                if(activitiesProblems.Count > 0)
                {
                    _dbContext.BulkInsert(activitiesProblems);
                }
            }
            catch (Exception e) when (e is DbUpdateException || e is DbUpdateConcurrencyException)
            {
                throw new DatabaseException("Ooops! We have some trouble with the database. Try again later.");
            }
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
