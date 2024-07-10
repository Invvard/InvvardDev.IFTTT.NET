variable "web_app_name" {
  type = string
}

variable "ifttt_service_key" {
  description = "Service key for IFTTT Service"
  type        = string
}

variable "tags" {
  description = "Tags to apply to all resources created."
  type        = map(string)

  default = {}
}
