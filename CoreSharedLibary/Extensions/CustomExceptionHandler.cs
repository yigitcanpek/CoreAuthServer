using CoreSharedLibary.DTO_s;
using CoreSharedLibary.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreSharedLibary.Extensions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure =>
            {
                //Use ile yazılan middlewareler devamlılık barındırır - Run ile yazılan middlewareler sonlandırıcıdır.
                configure.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerFeature errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        Exception middlewareException = errorFeature.Error;
                        ErrorDto errorDto = null;
                        if (middlewareException is CustomException)
                        {
                            errorDto = new ErrorDto(middlewareException.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(middlewareException.Message, false);
                        }

                        Response<NoDataDto> response = Response<NoDataDto>.Fail(errorDto, 500);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}
