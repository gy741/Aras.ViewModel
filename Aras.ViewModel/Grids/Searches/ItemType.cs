﻿/*
  Aras.ViewModel provides a .NET library for building Aras Innovator Applications

  Copyright (C) 2017 Processwall Limited.

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as published
  by the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see http://opensource.org/licenses/AGPL-3.0.
 
  Company: Processwall Limited
  Address: The Winnowing House, Mill Lane, Askham Richard, York, YO23 3NW, United Kingdom
  Tel:     +44 113 815 3440
  Email:   support@processwall.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aras.ViewModel.Grids.Searches
{
    public class ItemType : Search
    {
        protected override void AfterBindingChanged()
        {
            base.AfterBindingChanged();

            if (this.Binding != null)
            {
                if (this.Binding is Model.ItemType)
                {
                    this.Query = this.Session.Model.Store((Model.ItemType)this.Binding).Query();
                    this.Query.PageSize = (System.Int32)this.PageSize.Value;
                    this.Query.Paging = true;
                }
                else
                {
                    throw new Model.Exceptions.ArgumentException("Binding must be of type Aras.Model.ItemType");
                }
            }
            else
            {
                this.Query = null;
            }

            this.RefreshControl();
        }

        public Model.ObservableList<Model.Item> Selected { get; private set; }

        private Model.Queries.Item Query;

        private List<Model.PropertyType> PropertyTypes;

        protected override void LoadColumns()
        {
            if ((this.Binding != null) && (this.Binding is Model.ItemType))
            {
                this.Columns.Clear();

                // Build List of PropertyTypes
                this.PropertyTypes = new List<Model.PropertyType>();

                foreach (String propertyname in this.PropertyNames)
                {
                    if (((Model.ItemType)this.Binding).HasPropertyType(propertyname))
                    {
                        this.PropertyTypes.Add(((Model.ItemType)this.Binding).PropertyType(propertyname));
                    }
                }

                foreach (Model.PropertyType proptype in this.PropertyTypes)
                {
                    this.AddColumn(proptype.Name, proptype.Label);
                }
            }
            else
            {
                // Clear Columns
                this.Columns.Clear();
            }
        }

        private void LoadRows()
        {
            if (this.Query != null)
            {
                this.NoRows = this.Query.Count();

                for (int i = 0; i < this.NoRows; i++)
                {
                    Model.Item item = this.Query[i];

                    for (int j = 0; j < this.PropertyTypes.Count(); j++)
                    {
                        Model.PropertyType proptype = this.PropertyTypes[j];
                        Model.Property property = item.Property(proptype);

                        if (this.Rows[i].Cells[j].Value == null)
                        {
                            switch (property.GetType().Name)
                            {
                                case "String":
                                    this.Rows[i].Cells[j].Value = new Properties.String(this.Session);
                                    break;
                                case "Integer":
                                    this.Rows[i].Cells[j].Value = new Properties.Integer(this.Session);
                                    break;
                                default:
                                    throw new Model.Exceptions.ArgumentException("PropertyType not implmented: " + property.GetType().Name);
                            }
                        }

                        this.Rows[i].Cells[j].Value.Binding = property;
                    }
                }
            }
            else
            {
                // Clear all Rows
                this.NoRows = 0;
            }
        }

        protected override void RefreshControl()
        {
            base.RefreshControl();

            if (this.Query != null)
            {
                // Set Condition
                if (String.IsNullOrEmpty(this.QueryString.Value))
                {
                    this.Query.Condition = null;
                }
                else
                {
                    if (this.PropertyTypes.Count() == 1)
                    {
                        this.Query.Condition = Aras.Conditions.Like(this.PropertyTypes[0].Name, this.QueryString);
                    }
                    else
                    {
                        this.Query.Condition = Aras.Conditions.Or(Aras.Conditions.Like(this.PropertyTypes[0].Name, this.QueryString), Aras.Conditions.Like(this.PropertyTypes[1].Name, this.QueryString));

                        if (this.PropertyTypes.Count() > 2)
                        {
                            for (int i = 2; i < this.PropertyTypes.Count(); i++)
                            {
                                ((Model.Conditions.Or)this.Query.Condition).Add(Aras.Conditions.Like(this.PropertyTypes[i].Name, this.QueryString));
                            }
                        }
                    }
                }

                // Set PageSize and required Page
                this.Query.PageSize = (System.Int32)this.PageSize.Value;
                this.Query.Page = this.Page;

                // Refresh Query
                this.Query.Refresh();

                // Update NoPages
                this.NoPages = this.Query.NoPages;
            }
            else
            {
                this.NoPages = 0;
            }

            // Load Grid
            this.LoadRows();

            // Refresh Buttons
            this.NextPage.Refesh();
            this.PreviousPage.Refesh();
        }

        public ItemType(Manager.Session Session)
            :base(Session)
        {
            this.Selected = new Model.ObservableList<Model.Item>();
        }
    }
}