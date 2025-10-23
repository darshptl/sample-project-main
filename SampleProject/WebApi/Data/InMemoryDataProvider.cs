using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;

namespace WebApi.Data
{
    // simple in-memory store for demo purposes
    public sealed class InMemoryDataProvider
    {
        private static readonly Lazy<InMemoryDataProvider> _lazy =
            new Lazy<InMemoryDataProvider>(() => new InMemoryDataProvider());
        public static InMemoryDataProvider Instance => _lazy.Value;

        private readonly List<Product> _products = new List<Product>();
        private readonly List<Order> _orders = new List<Order>();

        private InMemoryDataProvider() { }

        // ---------- Product ----------
        public IEnumerable<Product> GetProducts() => _products;
        public Product GetProduct(Guid id) => _products.FirstOrDefault(p => p.Id == id);
        public void AddProduct(Product product) => _products.Add(product);
        public void UpdateProduct(Product product)
        {
            var existing = GetProduct(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
            }
        }
        public void DeleteProduct(Guid id)
        {
            var existing = GetProduct(id);
            if (existing != null) _products.Remove(existing);
        }

        //delete all products
        public void DeleteAllProducts()
        {
            _products.Clear();
        }

        // ---------- Order ----------
        public IEnumerable<Order> GetOrders() => _orders;
        public Order GetOrder(Guid id) => _orders.FirstOrDefault(o => o.Id == id);
        public void AddOrder(Order order) => _orders.Add(order);
        public void UpdateOrder(Order order)
        {
            var existing = GetOrder(order.Id);
            if (existing != null)
            {
                existing.CustomerName = order.CustomerName;
                existing.TotalAmount = order.TotalAmount;
            }
        }
        public void DeleteOrder(Guid id)
        {
            var existing = GetOrder(id);
            if (existing != null) _orders.Remove(existing);
        }

        //delete all orders
        public void DeleteAllOrders()
        {
            _orders.Clear();
        }
    }
}