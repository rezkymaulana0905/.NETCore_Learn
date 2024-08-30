import '../../constants/constants.dart';
import 'package:http/http.dart' as http;

class LoginProvider {
  final _url = "User";

  Future<http.Response> login(String username, String password) {
    return http.post(
      Url.parse("$_url/Login?username=$username&password=$password&address=-"),
    );
  }

  Future<http.Response> changePassword(String password, String token) {
    return http.post(
      Url.parse("$_url/ChangePassword?&password=$password"),
      headers: {'Authorization': "Bearer $token"},
    );
  }
}
