using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LearnKenKen.UIModels;
using LearnKenKen.Utilities;

namespace LearnKenKen.UserControls
{
    public class KenListEventArgs : EventArgs
    {
        public int SelectedIndex { get; set; }
    }

    partial class KenList : System.Web.UI.UserControl
    {
        public event EventHandler<KenListEventArgs> KenKenSelected;

        internal List<KenListItemViewModel> Items
        {
            set 
            {
                lstKenKens.DataSource = value;                
                lstKenKens.DataBind();
            }        
        }

        public int SelectKenKen 
        { 
            set 
            {
                lstKenKens.SelectedIndex = value;
                SelectItem(lstKenKens.SelectedIndex);
            } 
        }

        protected void lstKenKens_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectItem(lstKenKens.SelectedIndex);
        }

        private void SelectItem(int index)
        {
            LinkButton button = lstKenKens.Items[index].FindControlRecursive<LinkButton>();
            button.Font.Bold = true;

            if (KenKenSelected != null)
                KenKenSelected(this, new KenListEventArgs() { SelectedIndex = index });
        }

        protected void lstKenKens_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            LinkButton button = lstKenKens.Items[lstKenKens.SelectedIndex].FindControlRecursive<LinkButton>();
            button.Font.Bold = false;
        }
    }
}