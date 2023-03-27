using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace CompanyEmployees;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        /*
         * https://www.w3schools.com/charsets/ref_html_utf8.asp#:~:text=The%20Difference%20Between%20Unicode%20and,decimal%20numbers%20(code%20points).
         * 
         * The Difference Between Unicode and UTF-8:
         *   Unicode is a character set. UTF-8 is encoding.
         *   Unicode is a list of characters with unique decimal numbers (code points). A = 65, B = 66, C = 67, ....
         *   This list of decimal numbers represent the string "hello": 104 101 108 108 111
         *   Encoding is how these numbers are translated into binary numbers to be stored in a computer:
         *     UTF-8 encoding will store "hello" like this (binary): 01101000 01100101 01101100 01101100  01101111.
         *     
         *   Encoding translates numbers into binary. Character sets translates characters to numbers.
         */
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type type)
    {
        if (CanWorkWithCompanyDto(type))
        {
            return base.CanWriteType(type);
        }

        return false;

        static bool CanWorkWithCompanyDto(Type type) =>
            typeof(CompanyDto).IsAssignableFrom(type) ||
            typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type);
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        HttpResponse response = context.HttpContext.Response;
        StringBuilder buffer = new();

        if (context.Object is IEnumerable<CompanyDto> companies)
        {
            foreach (var company in companies)
            {
                FormatCsv(buffer, company);
            }
        }
        else
        {
            FormatCsv(buffer, (CompanyDto)context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company) =>
        buffer.AppendLine($"{company.Id}, '{company.Name}', '{company.FullAddress}'");
}
