#r @"./libs/Newtonsoft.Json.dll"
#r @"../../src/Farmer/bin/Debug/netstandard2.0/Farmer.dll"

open Farmer
open Farmer.Builders

let myStorage = storageAccount {
    name "mystorage"
    sku Storage.Premium_LRS
}

let s =
    { Name = ResourceName "mystorage"
      Sku = Storage.Premium_LRS
      EnableDataLake = false
      Containers = []
      FileShares = []
      Queues = Set.empty
      StaticWebsite = None }


let myWebApp = webApp {
    name "mysuperwebapp"
    sku WebApp.Sku.S1
    app_insights_off
    setting "storage_key" myStorage.Key
    depends_on myStorage.Name
}

let deployment = arm {
    location Location.NorthEurope
    add_resource myStorage
    add_resource myWebApp
    output "storage_key" myStorage.Key
    output "web_password" myWebApp.PublishingPassword
}

// deployment
// |> Deploy.execute "my-resource-group-name" Deploy.NoParameters
