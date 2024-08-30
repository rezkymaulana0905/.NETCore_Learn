import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class GuestProvider {
  final _url = "GuestRegist";

  Future<String> _getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token')!;
  }

  Future<http.Response> getData(String id) {
    return http.get(
      Url.parse("$_url/$id"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }

  Future<http.Response> patchData(String id, String total, String mode) {
    return http.patch(
      Url.parse("$_url/$mode/$id/$total"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }
}
