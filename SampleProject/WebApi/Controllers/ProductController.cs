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
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly InMemoryDataProvider _store = InMemoryDataProvider.Instance;

        [HttpPost, Route("create")]
        public HttpResponseMessage Create(Product product)
        {
            if (product == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Product is required");

            _store.AddProduct(product);
            return Found(product);
        }

        [HttpGet, Route("")]
        public HttpResponseMessage GetAll() => Found(_store.GetProducts().ToList());

        [HttpGet, Route("{id:guid}")]
        public HttpResponseMessage GetById(Guid id)
        {
            var product = _store.GetProduct(id);
            if (product == null) return Found($"The product you are trying to find, does not exist.");
            return Found(product);
        }

        [HttpPut, Route("{id:guid}/update")]
        public HttpResponseMessage Update(Guid id, Product product)
        {
            var existing = _store.GetProduct(id);
            if (existing == null) return Found($"The product you are trying to update, does not exist."); // or DoesNotExist();

            product.Id = id;
            _store.UpdateProduct(product);
            return Found(product);
        }

        [HttpDelete, Route("{id:guid}/delete")]
        public HttpResponseMessage Delete(Guid id)
        {
            var existing = _store.GetOrder(id);
            if (existing == null) return Found($"The product you are trying to delete, does not exist.");// or DoesNotExist();
            _store.DeleteProduct(id);
            return Found($"Product {id} deleted");
        }

        //delete all products
        [HttpDelete, Route("clear")]
        public HttpResponseMessage DeleteAllProducts()
        {
            _store.DeleteAllProducts();
            return Found();
        }
    }
}
