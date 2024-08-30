import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class EmployeeProvider {
  final _url = "TrafficOut";

  Future<String> _getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token')!;
  }

  Future<http.Response> getData(String id, String mode) {
    return http.get(
      Url.parse("$_url/$mode/$id"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }

  Future<http.Response> patchData(String id, String mode) {
    return http.patch(
      Url.parse("$_url/$mode/$id"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }
}
