import '../../constants/constants.dart';
import 'widgets/widgets.dart';
import 'login.dart';

class LoginPageLandscape extends GetView<LoginController> {
  const LoginPageLandscape({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      resizeToAvoidBottomInset: false,
      body: Obx(
        () => IndexedStack(
          alignment: Alignment.center,
          index: controller.pageIndex.value,
          children: const [
            _AuthPage(),
            _ChangePassword(),
          ],
        ),
      ),
    );
  }
}

class _AuthPage extends GetView<LoginController> {
  const _AuthPage();

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(child: _buildTitle()),
        Expanded(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              _buildBody(),
              LoginButton(
                onPressed: () => controller.login(),
                isLoading: controller.isLoading,
                text: "Login",
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildTitle() {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Text>[
        Text(
          "PTC",
          style: TextStyle(
              color: AppColor.theme,
              fontSize: baseWidth * 0.2,
              fontWeight: FontWeight.bold,
              height: 1),
        ),
        Text(
          "People Traffic Control",
          style: TextStyle(
            fontSize: baseWidth * 0.04,
            fontWeight: FontWeight.bold,
          ),
        ),
      ],
    );
  }

  Widget _buildBody() {
    return SizedBox(
      height: baseHeight * 0.4,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          Text(
            "Welcome to PTC,\nPlease Login First!",
            textAlign: TextAlign.center,
            style: TextStyle(
              fontSize: baseWidth * 0.05,
              fontWeight: FontWeight.w600,
            ),
          ),
          _buildForms(),
        ],
      ),
    );
  }

  Widget _buildForms() {
    return Column(
      children: [
        LoginForm(
          hintText: "Username",
          icon: Icons.person_outline,
          textController: controller.username,
        ),
        LoginForm(
          hintText: "Password",
          icon: Icons.lock_outline,
          textController: controller.password,
          isPasswordForm: true,
        ),
        TextButton(
          onPressed: () => controller.showResetMessage(),
          child: Text(
            "Forgot Password?",
            style: TextStyle(
              color: AppColor.theme,
              fontSize: baseWidth * 0.025,
              fontWeight: FontWeight.bold,
              decoration: TextDecoration.underline,
              decorationColor: AppColor.theme,
              decorationThickness: 2.5,
            ),
          ),
        ),
      ],
    );
  }
}

class _ChangePassword extends GetView<LoginController> {
  const _ChangePassword();

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(
          child: Padding(
            padding: EdgeInsets.symmetric(horizontal: baseWidth * 0.1),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                _buildTitle(),
                Container(
                  padding: const EdgeInsets.all(10),
                  decoration: BoxDecoration(
                    border: Border.all(color: AppColor.theme, width: 2),
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Text(
                    "When making new passwords, the new password must contain:\n"
                    "1. Uppercase and lowercase alphabetic letters\n"
                    "2. Numbers\n"
                    "3. Symbols (!\"#\$%&'()*+,-./:;<=>?@[\\]^_`{|}~)\n"
                    "4. At least 10 characters long\n"
                    "5. Passwords must be different from at least 3 last passwords\n",
                    style: TextStyle(fontSize: baseWidth * 0.02),
                  ),
                ),
              ],
            ),
          ),
        ),
        Expanded(
          child: Padding(
            padding: EdgeInsets.symmetric(horizontal: baseWidth * 0.1),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                Text(
                  "You're currently using the system default password.\n\n"
                  "For security purposes, please change your password before you continue.",
                  textAlign: TextAlign.center,
                  style: TextStyle(fontSize: baseWidth * 0.03),
                ),
                _buildForms(),
                LoginButton(
                  onPressed: () => controller.changePassword(),
                  isLoading: controller.isLoading,
                  text: "Change Password",
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildTitle() {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Text>[
        Text(
          "PTC",
          style: TextStyle(
              color: AppColor.theme,
              fontSize: baseWidth * 0.2,
              fontWeight: FontWeight.bold,
              height: 1),
        ),
        Text(
          "People Traffic Control",
          style: TextStyle(
            fontSize: baseWidth * 0.04,
            fontWeight: FontWeight.bold,
          ),
        ),
      ],
    );
  }

  Widget _buildForms() {
    return Column(
      children: [
        LoginForm(
          hintText: "New Password",
          icon: Icons.lock_outline,
          textController: controller.password,
          isPasswordForm: true,
        ),
        LoginForm(
          hintText: "Re-Type New Password",
          icon: Icons.lock_outline,
          textController: controller.rePassword,
          isPasswordForm: true,
        ),
      ],
    );
  }
}
