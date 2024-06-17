resource "azurerm_resource_group" "rg" {
  name     = "rg-iftttnet-sample-apps"
  location = "canadacentral"
  tags = {
    environment = "dev",
    owner       = "invvard"
  }
}

resource "azurerm_service_plan" "asp_iftttnet_sample_apps" {
  name                = "asp-iftttnet-sample-apps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "lwa_iftttnet_sample_trigger" {
  name                = "lwa-iftttnet-sample-trigger"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.asp_iftttnet_sample_apps.id

  https_only = true

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }
  }
}
