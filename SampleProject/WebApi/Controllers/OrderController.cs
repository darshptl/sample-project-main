using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Data;
using Data;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly InMemoryDataProvider _store = InMemoryDataProvider.Instance;

        [HttpPost, Route("create")]
        public HttpResponseMessage Create(Order order)
        {
            if (order == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Order is required");

            _store.AddOrder(order);
            return Found(order);
        }

        [HttpGet, Route("")]
        public HttpResponseMessage GetAll() => Found(_store.GetOrders().ToList());

        [HttpGet, Route("{id:guid}")]
        public HttpResponseMessage GetById(Guid id)
        {
            var order = _store.GetOrder(id);
            if (order == null) return Found($"The order you are trying to find, does not exist.");
            return Found(order);
        }

        [HttpPut, Route("{id:guid}/update")]
        public HttpResponseMessage Update(Guid id, Order order)
        {
            var existing = _store.GetOrder(id);
            if (existing == null) return Found($"The order you are trying to update, does not exist.");//DoesNotExist();
            order.Id = id;
            _store.UpdateOrder(order);
            return Found(order);
        }

        [HttpDelete, Route("{id:guid}/delete")]
        public HttpResponseMessage Delete(Guid id)
        {
            var existing = _store.GetOrder(id);
            if (existing == null) return Found($"The order you are trying to delete, does not exist.");//DoesNotExist();
            _store.DeleteOrder(id);
            return Found($"Order {id} deleted");
        }

        //delete all orders 
        [HttpDelete, Route("clear")]
        public HttpResponseMessage DeleteAllOrders()
        {
            _store.DeleteAllOrders();
            return Found();
        }
    }
}
