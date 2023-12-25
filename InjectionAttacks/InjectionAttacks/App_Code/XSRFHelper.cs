using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Optimization;
using System.Web.Providers.Entities;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InjectionAttacks
{
    public class XSRFHelper
    {
        /*
         * 1. Kullanıcı login olduğunda ve bu biçimde sayfada bulunduğunda, bir değer üret ve bunu hem istemci hem de sunucuda sakla.
         * 2. Kullanıcı bir istek gönderdiğinde, istemciye gönderilmiş değeri beraberinde getirsin.
         * 3. sunucuda bulunan değerle karşılaştırılsın.
         */
        public static void Check(Page page, HiddenField hiddenField)
        {
            if (!page.IsPostBack)
            {
                //Sayfanın normal açılma durumu.
                Guid guid = Guid.NewGuid();
                hiddenField.Value = guid.ToString();
                page.Session["token"] = guid;

            }
            else
            {
                //Butona tıklama gibi aynı sayfada olay fırlatarak çalışan fonksiyonlar.
                Guid client = new Guid(hiddenField.Value);
                Guid server = (Guid)page.Session["token"];
                if (client != server)
                {
                    throw new SecurityException("Seni çakal seniiiiiiiiiiii");
                }
            }
        }


    }
}