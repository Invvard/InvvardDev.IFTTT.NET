resource "azurerm_resource_group" "rg" {
  name     = "rg-iftttnet-sample-apps"
  location = "canadacentral"
  tags = {
    environment = "dev",
    owner       = "invvard"
  }
}

resource "azurerm_log_analytics_workspace" "law_iftttnet_sample_apps" {
  name                = "law-iftttnet-sample-apps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  sku               = "PerGB2018"
  retention_in_days = 30
}

resource "azurerm_application_insights" "appi_iftttnet_sample_apps" {
  name                = "asp-iftttnet-sample-apps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  workspace_id        = azurerm_log_analytics_workspace.law_iftttnet_sample_apps.id
  application_type    = "web"
}

resource "azurerm_service_plan" "asp_iftttnet_sample_apps" {
  name                = "asp-iftttnet-sample-apps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  os_type             = "Linux"
  sku_name            = "F1"
}

resource "azurerm_linux_web_app" "lwa_iftttnet_sample_trigger" {
  name                = var.web_app_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  service_plan_id = azurerm_service_plan.asp_iftttnet_sample_apps.id
  https_only      = true

  app_settings = {
    "ClientIftttOptions__ServiceKey" = var.ifttt_service_key,
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.appi_iftttnet_sample_apps.connection_string
  }

  site_config {
    always_on = false
    application_stack {
      dotnet_version = "8.0"
    }
  }
}
