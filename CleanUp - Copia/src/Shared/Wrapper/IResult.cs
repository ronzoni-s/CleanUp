using System.Collections.Generic;

namespace ErbertPranzi.Shared.Wrapper
{
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }

        string ErrorCode { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}