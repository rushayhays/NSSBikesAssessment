using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using System.Linq;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select b.Id, b.Brand, b.Color, Owner.Name AS OwnerName, Owner.Id AS OwnerId
                        FROM Bike b
                        LEFT JOIN Owner ON b.OwnerId = Owner.Id 
                    ";
                    var bikes = new List<Bike>();
                    // implement code here... 
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bike bike = new Bike()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            },
                            Color = reader.GetString(reader.GetOrdinal("Color"))
                        };
                        bikes.Add(bike);
                    }
                    reader.Close();
                    return bikes;
                }
            }
               
           
        }

        //public Bike GetBikeById(int id)
        //{
        //    Bike bike = null;
        //    //implement code here...
        //    return bike;
        //}

        public Bike GetBikeById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      Select b.Id, b.Brand, b.Color, Owner.Name AS OwnerName, Owner.Id AS OwnerId, Owner.Address, bt.Name as BikeName, wo.DateInitiated, wo.DateCompleted, wo.Description, wo.Id AS WorkOrderId
                        FROM Bike b
                        LEFT JOIN Owner ON b.OwnerId = Owner.Id 
                        LEFT JOIN BikeType bt ON bt.Id = b.BikeTypeId
                        LEFT JOIN WorkOrder wo ON b.Id = wo.BikeId
                        WHERE b.Id = 40
                    ";
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var bikes = new List<Bike>();
                        while (reader.Read())
                        {
                            var bikeId = reader.GetInt32(reader.GetOrdinal("Id"));

                            var existingBike = bikes.FirstOrDefault(p => p.Id == bikeId);
                            if (existingBike == null)
                            {
                                Bike bike = new Bike()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                    Owner = new Owner()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                        Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                        Address = reader.GetString(reader.GetOrdinal("Address"))
                                    },
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    BikeType = new BikeType()
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("BikeName"))
                                    },
                                    WorkOrders = new List<WorkOrder>()
                                };
                                bikes.Add(bike);

                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("WorkOrderId")))
                            {
                                existingBike.WorkOrders.Add(new WorkOrder()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("WorkOrderId")),
                                    DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                    DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted")),
                                    Description = reader.GetString(reader.GetOrdinal("Description"))
                                });
                            }
                        }
                        Bike bikeWithWorkOrders = bikes[0];
                        return bikeWithWorkOrders;
                    }
                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}
