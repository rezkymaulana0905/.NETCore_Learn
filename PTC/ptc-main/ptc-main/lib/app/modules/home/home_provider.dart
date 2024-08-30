import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class HomeProvider {
  final _url = "Recent";

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
}
