using CatalogueManagementService.LIb.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CatalogueManagementService.API.Controllers.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult SendApiError<T>(
            this ControllerBase ctrl,
            T? data,
            string errorSummary,
            ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(ms => ms.Value?.Errors.Any() == true)
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Remark = errorSummary,
                StatusCode = -2,
                Errors = errors.Select(kv => new ApiError
                {
                    ErrorCode = -100400,
                    FieldName = kv.Key,
                    ErrorMessage = string.Join(", ", kv.Value)
                }).ToList()
            };

            return ctrl.BadRequest(apiResponse);
        }

        public static IActionResult SendApiError<T>(
            this ControllerBase ctrl,
            T data,
            string fieldName,
            string errorDetails,
            int code)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Remark = errorDetails,
                StatusCode = -2,
                Errors = new List<ApiError>
            {
                new ApiError
                {
                    FieldName = fieldName,
                    ErrorMessage = errorDetails
                }
            }
            };

            return ctrl.StatusCode(code, apiResponse);
        }

        public static IActionResult SendApiError(
            this ControllerBase ctrl,
            string fieldName,
            string errorDetails,
            int code)
        {
            var apiResponse = new ApiResponse<object>
            {
                Data = null,
                Remark = errorDetails,
                StatusCode = -2,
                Errors = new List<ApiError>
            {
                new ApiError
                {
                    FieldName = fieldName,
                    ErrorMessage = errorDetails
                }
            }
            };

            return ctrl.StatusCode(code, apiResponse);
        }

        public static IActionResult SendApiResponse<T>(
            this ControllerBase ctrl,
            T? data,
            string? remarks)
            where T : class
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Remark = remarks,
                StatusCode = 1
            };

            return ctrl.StatusCode(StatusCodes.Status200OK, apiResponse);
        }
    }
}
