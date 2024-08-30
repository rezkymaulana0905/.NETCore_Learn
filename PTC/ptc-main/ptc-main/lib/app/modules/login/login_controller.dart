import '../../constants/constants.dart';
import '../../routes/routes.dart';
import '../../services/services.dart';
import 'login.dart';
import 'widgets/widgets.dart';

class LoginController extends GetxController
    with PasswordHandling, HandlingController {
  final username = TextEditingController();
  final password = TextEditingController();
  final rePassword = TextEditingController();

  final loginP = LoginProvider();
  String token = '';

  final pages = ["Authentication", "Change Password"];
  final pageIndex = 0.obs;

  void setPage(String title) {
    pageIndex.value = pages.indexWhere((element) => element == title);
  }

  final isLoading = false.obs;

  Future<void> login() async {
    if (username.text.isEmpty || password.text.isEmpty) {
      return showFail("Failed", "Please fill all the form");
    }

    isLoading.value = true;
    try {
      var data = await loginP.login(username.text, password.text);
      var body = jsonDecode(data.body);
      if (data.statusCode == 200) {
        token = body['token'];
        switch (body['result']) {
          case "Login":
            _setToken();
            Get.offNamed(Routes.home);
          case "ChangePassword":
            setPage("Change Password");
        }
      } else {
        if (body['title'] == null) {
          showFail("Error", "Error Occurred");
        } else if (body['title'] == "Account is Locked") {
          showLockedMessage();
        } else {
          showFail(body['title'], "");
        }
      }
    } catch (e) {
      showFail("Error", "Error Occurred");
    }
    isLoading.value = false;
  }

  Future<void> changePassword() async {
    if (!caseHandling(password.text)) {
      return showFail(
          "Failed", "Password doesn't contain uppercase and lowercase");
    }
    if (!numberHandling(password.text)) {
      return showFail("Failed", "Password doesn't contain number");
    }
    if (!symbolHandling(password.text)) {
      return showFail("Failed", "Password doesn't contain symbol");
    }
    if (!lengthHandling(password.text)) {
      return showFail("Failed", "Password is too short");
    }
    if (!rePasswordMatch(password.text, rePassword.text)) {
      return showFail("Failed", "Password doesn't match");
    }

    isLoading.value = true;
    try {
      var data = await loginP.changePassword(password.text, token);
      var body = jsonDecode(data.body);
      if (data.statusCode == 200) {
        token = body['token'];
        switch (body['result']) {
          case "Login":
            _setToken();
            Get.offNamed(Routes.home);
          case "ChangePassword":
            setPage("Change Password");
        }
      } else {
        body['title'] != null
            ? showFail(body['title'], "")
            : showFail("Error", "Error Occurred");
      }
    } catch (e) {
      showFail("Error", "Error Occurred");
    }
    isLoading.value = false;
  }

  void showLockedMessage() {
    _showMessageBox(
      const MessageBox(
        color: AppColor.leave,
        message:
            "Your account has been locked due to too many failed login attempts. "
            "Please contact IT support for further assistance.",
      ),
    );
  }

  void showResetMessage() {
    _showMessageBox(
      const MessageBox(
        color: AppColor.theme,
        message:
            "If you can't remember your password, you will need to reset it. "
            "Please contact IT support to access your account.",
      ),
    );
  }

  void _showMessageBox(MessageBox messageBox) {
    Get.dialog(
      barrierColor: Colors.transparent,
      barrierDismissible: false,
      Dialog(child: messageBox),
    );
  }

  Future<void> _setToken() async {
    final prefs = await SharedPreferences.getInstance();
    prefs.setString('token', token);
  }
}

mixin PasswordHandling {
  bool rePasswordMatch(String password, String rePassword) {
    return password == rePassword;
  }

  bool caseHandling(String password) {
    return password.contains(RegExp(r'(?=.*[a-z])(?=.*[A-Z])'));
  }

  bool numberHandling(String password) {
    return password.contains(RegExp(r'[0-9]'));
  }

  bool symbolHandling(String password) {
    return password.contains(RegExp(r'[!"#$%&()*+,-./:;<=>?@[\]^_`{|}~]')) ||
        password.contains("'");
  }

  bool lengthHandling(String password) {
    return password.length >= 10;
  }
}
