﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aras.ViewModel.Debug
{
    public class Session
    {
        private const String URL = "http://localhost/innovatorserver100sp4";

        public Boolean Execute()
        {
            // Create Server
            ViewModel.Server server = new Server(URL);

            // Add Applications
            server.LoadAssembly("Aras.ViewModel.Test");

            // Get Database
            ViewModel.Database database = server.Database("Development100SP4");

            // Craete Session
            ViewModel.Session session = database.Login("admin", "innovator");

            // Get Application
            Aras.ViewModel.Test.TestSearch testsearch = (Aras.ViewModel.Test.TestSearch)session.Applications.First();

            // Run Search
            while (true)
            {
                Boolean test = testsearch.Search.Refresh.Execute();

                System.Console.WriteLine("Item Count: " + testsearch.Search.Items.Count());

                foreach (Row row in testsearch.Search.Grid.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        System.Console.Write(((Properties.String)cell.Value).Value + " ");
                    }

                    System.Console.WriteLine("");
                }

                System.Threading.Thread.Sleep(2000);
            }

            return true;
        }

        public Session()
        {
        }
    }
}
