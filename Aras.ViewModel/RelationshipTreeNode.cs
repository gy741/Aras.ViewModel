﻿/*  
  Aras.ViewModel provides a .NET library for building Aras Innovator Applications

  Copyright (C) 2015 Processwall Limited.

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

namespace Aras.ViewModel
{
    public class RelationshipTreeNode : TreeNode
    {
        public RelationshipTree RelationshipTree
        {
            get
            {
                return (RelationshipTree)this.Tree;
            }
        }

        protected override void LoadChildren()
        {
            base.LoadChildren();


        }

        protected override void AfterBindingChanged()
        {
            base.AfterBindingChanged();

            if (this.Binding != null)
            {
                if (this.Binding is Model.Item)
                {
                    this.SetNameFromBinding();
                }
                else
                {
                    throw new Model.Exceptions.ArgumentException("Binding must be of type Model.Item");
                }
            }
            else
            {
                this.Name = "";
            }
        }

        public virtual void SetNameFromBinding()
        {
            if (this.Binding != null && this.Binding is Model.Item)
            {
                this.Name = (String)((Model.Item)this.Binding).Property("keyed_name").Value;
            }
        }

        public RelationshipTreeNode(RelationshipTree RelationshipTree)
            :base(RelationshipTree)
        {

        }
    }
}
