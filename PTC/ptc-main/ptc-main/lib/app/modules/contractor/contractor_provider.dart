import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class ContractorProvider {
  final _url = "WorkPermit";

  Future<String> _getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token')!;
  }

  Future<http.Response> getData(id) {
    return http.get(
      Url.parse("$_url/$id"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }

  Future<http.Response> patchData(id, mode) {
    return http.patch(
      Url.parse("$_url/$mode/$id"),
      headers: {'Authorization': "Bearer ${_getToken()}"},
    );
  }
}
