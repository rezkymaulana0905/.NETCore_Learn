abstract interface class Url {
  static const _domain = "http://10.83.34.6:8091";
  static const _api = "api";

  static String _getUrl(String endpoint) => "$_domain/$_api/$endpoint";
  static Uri parse(String endpoint) => Uri.parse(_getUrl(endpoint));
}