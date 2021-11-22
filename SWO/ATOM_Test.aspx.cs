using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel.Syndication;
using System.Xml;

public partial class ATOM_Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Atom10FeedFormatter formatter = new Atom10FeedFormatter();
            //XmlReader reader = XmlReader.Create("https://alerts.weather.gov/cap/fl.php?x=1");
            //SyndicationFeed feed = SyndicationFeed.Load(reader);

            //foreach (var item in feed.Items)
            //{
            //    Response.Write(item.Title.Text);
            //    Response.Write("<br />");
            //    Response.Write(item.Summary.Text);
            //    Response.Write("<br />");
            //    Response.Write(item.Id.Normalize());
            //    Response.Write("<br />");
            //    Response.Write(item.Id.Substring(0));
            //    Response.Write("<br />");
            //    Response.Write(item.Id.ToLower());
            //    Response.Write("<br />");
            //    Response.Write(item.Id.ToString());
            //}

            //reader.Close();

            XmlReader reader2 = XmlReader.Create("https://alerts.weather.gov/cap/wwacapget.php?x=FL125608AF2678.SpecialWeatherStatement.125608AF4A68FL.MLBSPSMLB.1bc500e175fb4fdc8607ad6ba11d0522");
            SyndicationFeed feed2 = SyndicationFeed.Load(reader2);

            //foreach (var item2 in feed2.Items)
            //{
            //    Response.Write(item2.Copyright.Text);
            //    Response.Write("<br />");
            //}

            //reader.Close();
        }
        catch (Exception x)
        {
            Response.Write(x.Message);
        }


    }
}