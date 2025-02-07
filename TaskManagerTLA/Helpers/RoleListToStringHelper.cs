﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Helpers
{
    public static class RoleListToStringHelper
    {
        public static HtmlString CreateString(this IHtmlHelper html, IEnumerable<RoleViewModel> items)
        {
            string result = "";
            var itemsArray = items.ToArray();
            for (int i = 0; i < itemsArray.Length; i++)
            {
                result += itemsArray[i].Name + (i < itemsArray.Length - 1 ? ", " : "");
            }
            TagBuilder tagP = new TagBuilder("p");
            tagP.InnerHtml.Append(result);
            var writer = new StringWriter();
            tagP.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}
