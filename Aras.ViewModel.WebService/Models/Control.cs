﻿/*  
  Copyright 2017 Processwall Limited

  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 
  Company: Processwall Limited
  Address: The Winnowing House, Mill Lane, Askham Richard, York, YO23 3NW, United Kingdom
  Tel:     +44 113 815 3440
  Web:     http://www.processwall.com
  Email:   support@processwall.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aras.ViewModel.WebService.Models
{
    public class Control : IEquatable<Control>
    {
        public String ID { get; set; }

        public String Type { get; set; }

        public List<Property> Properties { get; set; }

        public bool Equals(Control other)
        {
            if (other != null)
            {
                return this.ID.Equals(other.ID);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Control)
            {
                return this.Equals((Control)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public Control()
        {

        }

        public Control(String ID, String Type)
        {
            // Set ID
            this.ID = ID;
            
            // Set Type
            this.Type = Type;

            // Create Properties List
            this.Properties = new List<Property>();
        }
    }
}