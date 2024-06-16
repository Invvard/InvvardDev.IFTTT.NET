# Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = "rg-iftttnet-sample-apps"
  location = "canadacentral"
  tags = {
    environment = "dev",
    owner       = "invvard"
  }
}
