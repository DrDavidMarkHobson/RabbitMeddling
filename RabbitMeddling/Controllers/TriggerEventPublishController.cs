using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otb.Immutable;
using RabbitMeddling.Services;
using RabbitMeddling.Services.Events;

namespace RabbitMeddling.Controllers
{
    public class EventObject : DataContractImmutable<EventObject>
    {
        private readonly int _id;
        public EventObject(int id)
        {
            _id = id;
        }
        public int Id { get; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TriggerEventPublishController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;

        public TriggerEventPublishController(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        [HttpGet]
        public ActionResult Get()
        {
            _eventPublisher.Publish(
                new Event<string>(
                    "Test",
                    "Test"));

            return new OkResult();
        }
        [HttpGet]
        [Route("~/api/[controller]/id/{id}")]
        public ActionResult Get(int id)
        {
            _eventPublisher.Publish(
                new Event<EventObject>(
                    new EventObject(id), 
                    "id"));

            return new OkResult();
        }


    }
}
