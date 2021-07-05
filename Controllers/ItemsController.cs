using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IInMemItemsRepository repository;
        public ItemsController(IInMemItemsRepository repository)
        {
            this.repository = repository;

        }
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            return repository.GetItems().Select(item => item.AsDto());
        }
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = repository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Price = updateItemDto.Price
            };
            repository.UpdateItem(updatedItem);
            return NoContent();

        }
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id){
                 var existingItem = repository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }  
            repository.DeleteItem(id);
            return NoContent();  
        }
    }
}