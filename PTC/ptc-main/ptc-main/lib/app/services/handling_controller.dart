import '../constants/constants.dart';

mixin HandlingController {
  void showPass(String title, String text) => _showSnackBar(title, text, AppColor.enter);
  void showFail(String title, String text) => _showSnackBar(title, text, AppColor.leave);

  void _showSnackBar(String title, String message, Color color) {
    Get.snackbar(
      title,
      message,
      snackPosition: SnackPosition.TOP,
      backgroundColor: color,
      borderRadius: 20,
      margin: const EdgeInsets.all(15),
      colorText: Colors.white,
      duration: const Duration(seconds: 2),
      isDismissible: true,
      forwardAnimationCurve: Curves.easeOutBack,
    );
  }
}