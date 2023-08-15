namespace ECOM.Services.CouponAPI.Middlewares;
public class ExceptionHandlingMiddelware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await next(context);
		}
		catch (Exception)
		{

			throw;
		}
    }
}
