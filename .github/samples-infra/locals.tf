locals {
  web_app_dynamic_tags = tomap({
    "hidden-link: /app-insights-resource-id"         = replace(azurerm_application_insights.appi_iftttnet_sample_apps.id, "Microsoft.Insights", "microsoft.insights")
    "hidden-link: /app-insights-conn-string"         = azurerm_application_insights.appi_iftttnet_sample_apps.connection_string
    "hidden-link: /app-insights-instrumentation-key" = azurerm_application_insights.appi_iftttnet_sample_apps.instrumentation_key
  })

  rg_default_tags = {
    environment = "dev",
    owner       = "invvard"
  }
}
