import '../constants/constants.dart';
import '../services/services.dart';
import 'widgets.dart';

class ScanPage extends StatelessWidget {
  const ScanPage({super.key, required this.controller});

  final BaseController controller;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: MyAppBar(
        title: "${Get.arguments['menu']} ${Get.arguments['mode']}",
        onTap: () => Get.back(),
      ),
      body: Stack(children: <Widget>[
        Builder(builder: (context) => _buildErrorText(context.orientation)),
        _buildScanButton()
      ]),
    );
  }

  Widget _buildErrorText(Orientation orientation) {
    return Align(
      alignment: Alignment.topCenter,
      child: Padding(
        padding: EdgeInsets.only(
            top: switch (orientation) {
          Orientation.portrait => baseHeight * 0.1,
          Orientation.landscape => baseHeight * 0.025,
        }),
        child: Obx(() {
          return Text(
            controller.isError() ? controller.error.value : "",
            style: TextStyle(
              color: AppColor.leave,
              fontSize: baseWidth * 0.075,
              fontWeight: FontWeight.w500,
            ),
          );
        }),
      ),
    );
  }

  Widget _buildScanButton() {
    final isPressed = false.obs;
    return Center(
      child: GestureDetector(
        onTapCancel: () => isPressed.value = !isPressed.value,
        onTapDown: (_) => isPressed.value = !isPressed.value,
        onTapUp: (_) {
          isPressed.value = !isPressed.value;
          controller.changeScannerMode();
        },
        child: Obx(
          () => AnimatedContainer(
            duration: const Duration(milliseconds: 50),
            padding: EdgeInsets.all(baseWidth * 0.1),
            width: isPressed.value ? baseWidth * 0.5 * 0.95 : baseWidth * 0.5,
            height: isPressed.value ? baseWidth * 0.5 * 0.95 : baseWidth * 0.5,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: Get.arguments['color'],
              boxShadow: [
                if (!isPressed.value)
                  const BoxShadow(
                    color: AppColor.shadow,
                    blurRadius: 5,
                    spreadRadius: 3,
                  ),
              ],
            ),
            child: Obx(() => controller.isLoading.value
                ? const CircularProgressIndicator(
                    color: Colors.white,
                    strokeWidth: 40,
                  )
                : Image.asset("assets/icons/scan_icon.png", fit: BoxFit.fill)),
          ),
        ),
      ),
    );
  }
}
