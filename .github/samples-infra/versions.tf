terraform {
  backend "azurerm" {
    resource_group_name  = "rg-iftttnet-samples-backend"
    storage_account_name = "saiftttnettfbackend"
    container_name       = "tfstates"
    key                  = "terraform.tfstate"
    use_oidc             = true
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.111.0"
    }
  }
}

provider "azurerm" {
  features {}
  use_oidc = true
}
