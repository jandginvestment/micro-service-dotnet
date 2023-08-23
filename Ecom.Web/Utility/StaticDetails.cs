namespace ECOM.Web.Utility;
    public class StaticDetails
    {
    public static string? ProductAPIBase { get; set; }
    public static string? CouponAPIBase { get; set; }

    public static string? AuthAPIBase { get; set; }
    public static string RoleCustomer = "CUSTOMER";
    public static string RoleAdmin = "ADMIN";
    public static string AssignRoleSuccess = "Role assigned successfully";
    public static string Token = "JWT";

    public enum APIType { GET, POST, PUT, DELETE }
    }
