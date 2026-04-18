namespace XtxServer.DTOs;

public class ApiResponse<T>
{
    public string Msg { get; set; } = "操作成功";
    public T? Result { get; set; }
    public int Code { get; set; } = 200;

    public static ApiResponse<T> Success(T result, string msg = "操作成功")
    {
        return new ApiResponse<T> { Result = result, Msg = msg, Code = 200 };
    }

    public static ApiResponse<T> Error(string msg, int code = 500)
    {
        return new ApiResponse<T> { Msg = msg, Code = code };
    }
}

public class ApiResponse
{
    public string Msg { get; set; } = "操作成功";
    public int Code { get; set; } = 200;

    public static ApiResponse Success(string msg = "操作成功")
    {
        return new ApiResponse { Msg = msg, Code = 200 };
    }

    public static ApiResponse Error(string msg, int code = 500)
    {
        return new ApiResponse { Msg = msg, Code = code };
    }
}
