using AutoMapper;
using ShoppingList.Business.Repositories.IRepositories;
using ShoppingList.Data.Entities;
using ShoppingList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ShoppingList.WebAPI.Endpoints
{
    public static class ShoppingListEndpoints
    {
        public static void ConfigureListItemEndpoints(this WebApplication app)
        {
            app.MapGet("/api/item", GetAllItems)
                .WithName("GetItems")
                .Produces<APIResponse>(200);
            //.RequireAuthorization();
            //.RequireAuthorization("AdminOnly");

            app.MapGet("/api/item/{id:int}", GetItem)
                .WithName("GetCoupon")
                .Produces<APIResponse>(200);

            app.MapPost("/api/item", CreateItem)
                .WithName("CreateCoupon")
                .Accepts<ListItemCreateDTO>("application/json")
                .Produces<APIResponse>(201)
                .Produces(400);

            app.MapPut("/api/item", UpdateItem)
                .WithName("UpdateCoupon")
                .Accepts<ListItemUpdateDTO>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400);

            app.MapDelete("/api/item/{id:int}", DeleteItem);
        }

        private async static Task<IResult> DeleteItem(IListItemRepository listItemRepository, int id)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            ListItemDTO listItemFromStore = await listItemRepository.GetAsync(id);
            if (listItemFromStore != null)
            {
                await listItemRepository.RemoveAsync(listItemFromStore.Id);
                await listItemRepository.SaveAsync();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);
            }
            else
            {
                response.ErrorMessages.Add("Invalid Id");
                return Results.BadRequest(response);
            }
        }

        private async static Task<IResult> UpdateItem(
            IListItemRepository listItemRepository,
            IMapper mapper,
            [FromBody] ListItemUpdateDTO listItem_U_DTO)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            await listItemRepository.UpdateAsync(mapper.Map<ListItemDTO>(listItem_U_DTO));
            await listItemRepository.SaveAsync();

            response.Result = mapper.Map<ListItemDTO>(await listItemRepository.GetAsync(listItem_U_DTO.Id)); ;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }


        private async static Task<IResult> CreateItem(
            IListItemRepository listItemRepository,
            IMapper mapper,
            [FromBody] ListItemCreateDTO listItem_C_DTO)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            if (listItemRepository.GetAsync(listItem_C_DTO.Product).GetAwaiter().GetResult() != null)
            {
                response.ErrorMessages.Add("Coupon name already exists");
                return Results.BadRequest(response);
            }

            await listItemRepository.CreateAsync(mapper.Map<ListItemDTO>(listItem_C_DTO));
            await listItemRepository.SaveAsync();

            response.Result = listItem_C_DTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);

            //return Results.CreatedAtRoute("GetCoupon",new { id=coupon.Id }, couponDTO);
            //return Results.Created($"/api/coupon/{coupon.Id}",coupon);
        }

        [Authorize]
        private async static Task<IResult> GetAllItems(
            IListItemRepository _listItemRepo, ILogger<Program> _logger)
        {
            APIResponse response = new();
            _logger.Log(LogLevel.Information, "Getting all coupons");
            response.Result = await _listItemRepo.GetAllAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

        private async static Task<IResult> GetItem(
            IListItemRepository _listItemRepo, ILogger<Program> _logger, int id)
        {
            APIResponse response = new();
            response.Result = await _listItemRepo.GetAsync(id);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

    }
}
