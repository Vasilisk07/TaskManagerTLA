using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.Helpers
{
    public static class SetRolesHelper
    {


        //public static HtmlString CreateList(this IHtmlHelper html, string[] items)
        //{
        //    TagBuilder div = new TagBuilder("div");
        //    for (int i=0; i< items.Length; i++)
        //    {
        //        TagBuilder input = new TagBuilder("input");
        //        input.Attributes.Add("type", "checkbox");

        //    }
        //    ul.Attributes.Add("class", "itemsList");
        //    var writer = new System.IO.StringWriter();
        //    ul.WriteTo(writer, HtmlEncoder.Default);
        //    return new HtmlString(writer.ToString());
        //}

    }
}
