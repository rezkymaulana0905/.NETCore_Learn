import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class SupplierProvider {
  final _url = "Spplier";

  Future<String> _getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token')!;
  }

  Future<http.Response> getData() {
    return http.get(
      Url.parse(_url),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }

  Future<http.Response> postData(data) {
    return http.post(
      Url.parse(_url),
      body: jsonEncode(data),
      headers: {
        'Authorization': "Bearer ${_getToken()}",
        'Content-Type': 'application/json; charset=UTF-8'
      },
    );
  }

  Future<http.Response> patchData(data) {
    return http.patch(
      Url.parse(_url),
      body: jsonEncode(data),
      headers: {
        'Authorization': "Bearer ${_getToken()}",
        'Content-Type': 'application/json; charset=UTF-8'
      },
    );
  }
}
