import '../constants/constants.dart';
import '../routes/routes.dart';

class SplashScreen extends StatelessWidget {
  SplashScreen({super.key}) {
    _checkToken();
  }

  void _checkToken() {
    Future.delayed(const Duration(seconds: 2), () async {
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('token');
      if (token == null) {
        Get.offNamed(Routes.login);
      } else {
        Get.offNamed(Routes.home);
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <FittedBox>[
            FittedBox(
              child: Text(
                "PTC",
                style: TextStyle(
                  color: AppColor.theme,
                  fontWeight: FontWeight.bold,
                  fontSize: 125,
                  height: 1,
                ),
              ),
            ),
            FittedBox(
              child: Text(
                "People Traffic Control",
                style: TextStyle(fontWeight: FontWeight.bold, fontSize: 25),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
