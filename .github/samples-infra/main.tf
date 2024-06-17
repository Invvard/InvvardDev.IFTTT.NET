# Create a resource group
resource "azurerm_resource_group" "rg" {
  name     = "rg-iftttnet-sample-apps"
  location = "canadacentral"
  tags = {
    environment = "dev",
    owner       = "invvard"
  }
}

# Create an App Service Plan
resource "azurerm_app_service_plan" "asp_iftttnet_sample_apps" {
  name                = "asp-iftttnet-sample-apps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Free"
    size = "F1"
  }
}

# Create an App Service
resource "azurerm_app_service" "as_iftttnet_sample_trigger" {
  name                = "as-iftttnet-sample-trigger"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp_iftttnet_sample_apps.id

  https_only = true

  site_config {
    linux_fx_version = "DOTNETCORE|8.0"
    scm_type         = "GitHub"
  }
}
